using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DnsZone.Tokens;

namespace DnsZone {
    public class DnsZone {

        public IList<DnzZoneInclude> Includes { get; } = new List<DnzZoneInclude>();

        public static DnsZone Parse(string content) {
            var zone = new DnsZone();
            var tokenizer = new Tokenizer();
            var source = new FileSource {
                Content = content
            };
            var tokens = tokenizer.Read(source).ToArray();
            var context = new DnsZoneParseContext(zone, tokens);
            while (!context.IsEof) {
                var token = context.Tokens.Dequeue();
                switch (token.Type) {
                    case TokenType.NewLine:
                        break;
                    case TokenType.Control:
                        ProcessControlDirective(context, token.StringValue);
                        break;
                    case TokenType.Literal:
                        break;
                }
            }
            return zone;
        }

        private static void ProcessControlDirective(DnsZoneParseContext context, string directive) {
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
                    context.DefaultTtl = ParseTtl(context.Tokens.Dequeue().StringValue);
                    break;
                default:
                    throw new NotSupportedException($"Unknown control directive '{directive}'");
            }
        }

        private static TimeSpan ParseTtl(string val) {
            var res = TimeSpan.Zero;
            int? part = null;
            foreach (var ch in val) {
                if (ch >= '0' && ch <= '9') {
                    part = (part ?? 0) * 10 + (ch - '0');
                } else {
                    if (part == null) throw new Exception("TTL value expected");
                    switch (char.ToUpper(ch)) {
                        case 'w':
                            res += TimeSpan.FromDays(part.Value * 7);
                            break;
                        case 'd':
                            res += TimeSpan.FromDays(part.Value);
                            break;
                        case 'h':
                            res += TimeSpan.FromHours(part.Value);
                            break;
                        case 'm':
                            res += TimeSpan.FromMinutes(part.Value);
                            break;
                        case 's':
                            res += TimeSpan.FromSeconds(part.Value);
                            break;
                    }
                    part = null;
                }
            }
            if (part != null) {
                res += TimeSpan.FromSeconds(part.Value);
            }
            return res;
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
