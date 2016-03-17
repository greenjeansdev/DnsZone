using System;
using System.Collections.Generic;
using System.Net;
using DnsZone.Records;
using DnsZone.Tokens;

namespace DnsZone.Parser {
    public class DnsZoneParseContext {

        public string Origin { get; set; }

        public TimeSpan? DefaultTtl { get; set; }

        public string DefaultClass { get; set; }

        public DnsZone Zone { get; }

        public Queue<Token> Tokens { get; }

        public bool IsEof => Tokens.Count == 0;

        public DnsZoneParseContext(DnsZone zone, IEnumerable<Token> tokens) {
            Zone = zone;
            Tokens = new Queue<Token>(tokens);
        }

        public ushort ReadPreference() {
            var token = Tokens.Dequeue();
            if (token.Type != TokenType.Literal) throw new TokenException("preference expected", token);
            return ushort.Parse(token.StringValue);
        }

        public string ReadDomainName() {
            var token = Tokens.Dequeue();
            if (token.Type != TokenType.Literal) throw new TokenException("domain name expected", token);
            return token.StringValue;
        }

        public string ReadEmail() {
            var token = Tokens.Dequeue();
            if (token.Type != TokenType.Literal) throw new TokenException("email expected", token);
            return token.StringValue;
        }

        public string ReadSerialNumber() {
            var token = Tokens.Dequeue();
            if (token.Type != TokenType.Literal) throw new TokenException("serial number expected", token);
            return token.StringValue;
        }

        public IPAddress ReadIpAddress() {
            var token = Tokens.Dequeue();
            if (token.Type != TokenType.Literal) throw new TokenException("ipaddress expected", token);
            return IPAddress.Parse(token.StringValue);
        }

        public TimeSpan ReadTimeSpan() {
            var val = Tokens.Dequeue().StringValue;
            return DnsZoneUtils.ParseTimeSpan(val);
        }

        public ResourceRecordType ReadResourceRecordType() {
            var token = Tokens.Dequeue();
            if (token.Type != TokenType.Literal) throw new TokenException("resource record type expected", token);
            return DnsZoneUtils.ParseResourceRecordType(token.StringValue);
        }


        public TimeSpan GetTimeSpan(TimeSpan? explicitValue) {
            if (explicitValue.HasValue) return explicitValue.Value;
            if (DefaultTtl.HasValue) return DefaultTtl.Value;
            throw new Exception("unknown ttl value");
        }

        public string GetClass(string explicitValue) {
            if (explicitValue != null) return explicitValue;
            if (DefaultClass != null) return DefaultClass;
            throw new Exception("unknown class value");
        }

        public string GetDomainName(string explicitValue) {
            if (explicitValue != null) return explicitValue;
            if (Origin != null) return Origin;
            throw new Exception("unknown domain name value");
        }

        public bool TryParseTtl(out TimeSpan val) {
            val = TimeSpan.Zero;
            var token = Tokens.Peek();
            if (token.Type != TokenType.Literal) return false;

            if (DnsZoneUtils.TryParseTimeSpan(token.StringValue, out val)) {
                Tokens.Dequeue();
                return true;
            }

            return false;
        }

        public bool TryParseTtl(out TimeSpan? timestamp) {
            TimeSpan val;
            if (TryParseTtl(out val)) {
                timestamp = val;
                return true;
            }
            timestamp = null;
            return false;
        }

        public bool TryParseClass(out string @class) {
            @class = null;
            var token = Tokens.Peek();
            if (token.Type != TokenType.Literal) return false;
            if (token.StringValue.ToUpperInvariant() == "IN") {
                @class = "IN";
                Tokens.Dequeue();
                return true;
            }
            return false;
        }

        private void SkipWhiteAndComments() {
            while (!IsEof) {
                var token = Tokens.Peek();
                switch (token.Type) {
                    case TokenType.Whitespace: 
                    case TokenType.Comments:
                        Tokens.Dequeue();
                        break;
                    default:
                        return;
                }
            }
        }
    }
}
