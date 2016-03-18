using System;
using System.Net;
using System.Text;

namespace DnsZone.Formatter {
    public class DnsZoneFormatterContext {

        private const string TAB_CHAR = "\t";

        public string Origin { get; set; }

        public TimeSpan? DefaultTtl { get; set; }


        public string PrevName { get; set; }

        public string PrevClass { get; set; }


        public DnsZone Zone { get; }

        public StringBuilder Sb { get; }

        public DnsZoneFormatterContext(DnsZone zone, StringBuilder sb) {
            Sb = sb;
            Zone = zone;
        }

        public void WritePreference(ushort val) {
            Sb.Append(val);
            Sb.Append(TAB_CHAR);
        }

        public void WriteDomainName(string val) {
            Sb.Append(val);
            Sb.Append(TAB_CHAR);
        }

        public void WriteAndCompressDomainName(string val) {
            WriteDomainName(CompressDomainName(val));
        }

        public void WriteEmail(string val) {
            Sb.Append(val);
            Sb.Append(TAB_CHAR);
        }

        public void WriteSerialNumber(string val) {
            Sb.Append(val);
            Sb.Append(TAB_CHAR);
        }

        public void WriteIpAddress(IPAddress val) {
            Sb.Append(val);
            Sb.Append(TAB_CHAR);
        }

        public void WriteTimeSpan(TimeSpan val) {
            Sb.Append(DnsZoneUtils.FormatTimeSpan(val));
            Sb.Append(TAB_CHAR);
        }

        public void WriteString(string val) {
            val = val
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"");
            Sb.Append(val);
            Sb.Append(TAB_CHAR);
        }

        public void WriteClass(string val) {
            Sb.Append(val);
            Sb.Append(TAB_CHAR);
        }

        //public ResourceRecordType ReadResourceRecordType() {
        //    var token = Tokens.Dequeue();
        //    if (token.Type != TokenType.Literal) throw new TokenException("resource record type expected", token);
        //    return DnsZoneUtils.ParseResourceRecordType(token.StringValue);
        //}

        public string CompressDomainName(string val) {
            if (val == Origin) {
                return "@";
            }
            if (Origin != null && val.EndsWith(Origin)) {
                return val.Substring(0, val.Length - Origin.Length);
            }
            return val + ".";
        }

        //public TimeSpan GetTimeSpan(TimeSpan? explicitValue) {
        //    if (explicitValue.HasValue) return explicitValue.Value;
        //    if (DefaultTtl.HasValue) return DefaultTtl.Value;
        //    throw new Exception("unknown ttl value");
        //}

        //public bool TryParseTtl(out TimeSpan val) {
        //    val = TimeSpan.Zero;
        //    var token = Tokens.Peek();
        //    if (token.Type != TokenType.Literal) return false;

        //    if (DnsZoneUtils.TryParseTimeSpan(token.StringValue, out val)) {
        //        Tokens.Dequeue();
        //        return true;
        //    }

        //    return false;
        //}

        //public bool TryParseTtl(out TimeSpan? timestamp) {
        //    TimeSpan val;
        //    if (TryParseTtl(out val)) {
        //        timestamp = val;
        //        return true;
        //    }
        //    timestamp = null;
        //    return false;
        //}

        //public bool TryParseClass(out string @class) {
        //    @class = null;
        //    var token = Tokens.Peek();
        //    if (token.Type != TokenType.Literal) return false;
        //    if (DnsZoneUtils.TryParseClass(token.StringValue, out @class)) {
        //        Tokens.Dequeue();
        //        return true;
        //    }
        //    return false;
        //}

    }
}
