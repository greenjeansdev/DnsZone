using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DnsZone.Parser;
using DnsZone.Records;
using DnsZone.Tokens;

namespace DnsZone {
    public class DnsZone {

        private static ResourceRecordReader _reader = new ResourceRecordReader();

        public IList<DnzZoneInclude> Includes { get; } = new List<DnzZoneInclude>();

        public IList<ResourceRecord> Records { get; } = new List<ResourceRecord>();

        public static DnsZone Parse(string content) {
            var zone = new DnsZone();
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
                    context.Origin = context.Tokens.Dequeue().StringValue;
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

            var name = context.Tokens.Peek().Type == TokenType.Whitespace ? null : context.ReadDomainName();

            if (context.TryParseClass(out @class)) {
                context.TryParseTtl(out ttl);
            } else if (context.TryParseTtl(out ttl)) {
                context.TryParseClass(out @class);
            }

            var type = context.ReadResourceRecordType();

            var record = DnsZoneUtils.CreateRecord(type);
            record.Name = context.GetDomainName(name);
            record.Class = context.GetClass(@class);
            record.Ttl = context.GetTimeSpan(ttl);

            record.AcceptVistor(_reader, context);

            context.Zone.Records.Add(record);

            context.DefaultClass = record.Class;
        }

        public static async Task<DnsZone> LoadAsync(string uri) {
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
