using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DnsZone.Formatter;
using DnsZone.Parser;
using DnsZone.Records;
using DnsZone.Tokens;

namespace DnsZone {
    public class DnsZoneFile {

        private static readonly ResourceRecordReader _reader = new ResourceRecordReader();

        public IList<DnzZoneInclude> Includes { get; } = new List<DnzZoneInclude>();

        public IList<ResourceRecord> Records { get; } = new List<ResourceRecord>();

        public override string ToString() {
            var sb = new StringBuilder();
            var context = new DnsZoneFormatterContext(this, sb);
            var writer = new ResourceRecordWriter();
            foreach (var recordGroup in Records.GroupBy(item => item.Type)) {
                context.Sb.AppendLine($";{recordGroup.Key} records");
                foreach (var record in recordGroup) {
                    context.WriteAndCompressDomainName(record.Name);
                    context.WriteClass(record.Class);
                    context.WriteTimeSpan(record.Ttl);
                    record.AcceptVistor(writer, context);
                    context.Sb.AppendLine();
                }
                context.Sb.AppendLine();
            }
            return sb.ToString();
        }

        public static DnsZoneFile Parse(string content) {
            var zone = new DnsZoneFile();
            var tokenizer = new Tokenizer();
            var source = new FileSource {
                Content = content
            };
            var tokens = tokenizer.Read(source).ToArray();
            var context = new DnsZoneParseContext(zone, tokens);
            while (!context.IsEof) {
                var token = context.Tokens.Peek();
                switch (token.Type) {
                    case TokenType.NewLine:
                        context.Tokens.Dequeue();
                        break;
                    case TokenType.Control:
                        ProcessControlDirective(context);
                        break;
                    case TokenType.Whitespace:
                    case TokenType.Literal:
                        ParseResourceRecord(context);
                        break;
                    default:
                        throw new NotSupportedException($"not supported type {token.Type}");
                }
            }
            return zone;
        }

        private static void ProcessControlDirective(DnsZoneParseContext context) {
            var token = context.Tokens.Dequeue();
            var directive = token.StringValue;
            switch (directive.ToUpperInvariant()) {
                case "ORIGIN":
                    context.Origin = context.ReadAndResolveDomainName();
                    break;
                case "INCLUDE":
                    var firstToken = context.Tokens.Dequeue();
                    var secondToken = context.Tokens.Dequeue();
                    if (secondToken.Type == TokenType.NewLine) {
                        context.Zone.Includes.Add(new DnzZoneInclude {
                            Origin = context.Origin,
                            FileName = firstToken.StringValue
                        });
                    } else {
                        context.Tokens.Dequeue(); //end of line
                        context.Zone.Includes.Add(new DnzZoneInclude {
                            Origin = secondToken.StringValue,
                            FileName = firstToken.StringValue
                        });
                    }
                    break;
                case "TTL":
                    context.DefaultTtl = context.ReadTimeSpan();
                    break;
                default:
                    throw new NotSupportedException($"Unknown control directive '{directive}'");
            }
        }

        private static void ParseResourceRecord(DnsZoneParseContext context) {
            string @class;
            TimeSpan? ttl;

            var nameToken = context.Tokens.Dequeue();
            var name = nameToken.Type != TokenType.Whitespace ? nameToken.StringValue : "";

            if (context.TryParseClass(out @class)) {
                context.TryParseTtl(out ttl);
            } else if (context.TryParseTtl(out ttl)) {
                context.TryParseClass(out @class);
            }

            var type = context.ReadResourceRecordType();

            var record = DnsZoneUtils.CreateRecord(type);
            record.Name = context.ResolveDomainName(!string.IsNullOrWhiteSpace(name) ? name : context.PrevName);
            record.Class = !string.IsNullOrWhiteSpace(@class) ? @class : context.PrevClass;
            record.Ttl = context.GetTimeSpan(ttl);

            record.AcceptVistor(_reader, context);

            context.Zone.Records.Add(record);

            context.PrevName = name;

            if (!string.IsNullOrWhiteSpace(@class)) {
                context.PrevClass = @class;
            }

        }

        public static async Task<DnsZoneFile> LoadAsync(string uri) {
            var request = WebRequest.Create(uri);
            using (var response = await request.GetResponseAsync()) {
                var stream = response.GetResponseStream();
                if (stream == null) throw new Exception("Content not found");
                using (stream) {
                    var content = await new StreamReader(stream).ReadToEndAsync();
                    return Parse(content);
                }
            }
        }
    }
}
