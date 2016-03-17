using System;
using DnsZone.Records;

namespace DnsZone {
    public static class DnsZoneUtils {

        public static ResourceRecordType ParseResourceRecordType(string val) {
            switch (val.ToUpperInvariant()) {
                case "A": return ResourceRecordType.A;
                case "AAAA": return ResourceRecordType.AAAA;
                case "NS": return ResourceRecordType.NS;
                case "MD": return ResourceRecordType.MD;
                case "MF": return ResourceRecordType.MF;
                case "CNAME": return ResourceRecordType.CNAME;
                case "SOA": return ResourceRecordType.SOA;
                case "MB": return ResourceRecordType.MB;
                case "MG": return ResourceRecordType.MG;
                case "MR": return ResourceRecordType.MR;
                case "NULL": return ResourceRecordType.NULL;
                case "WKS": return ResourceRecordType.WKS;
                case "PTR": return ResourceRecordType.PTR;
                case "HINFO": return ResourceRecordType.HINFO;
                case "MX": return ResourceRecordType.MX;
                case "TXT": return ResourceRecordType.TXT;
                default:
                    throw new NotSupportedException($"unsupported resource record type {val}");
            }
        }

        public static ResourceRecord CreateRecord(ResourceRecordType type) {
            switch (type) {
                case ResourceRecordType.A: return new AResourceRecord();
                case ResourceRecordType.AAAA: return new AAAAResourceRecord();
                case ResourceRecordType.NS: return new NsResourceRecord();
                //case ResourceRecordType.MD: return ResourceRecordType.MD;
                //case ResourceRecordType.MF: return ResourceRecordType.MF;
                //case ResourceRecordType.CNAME: return ResourceRecordType.CNAME;
                case ResourceRecordType.SOA: return new SoaResourceRecord();
                //case ResourceRecordType.MB: return ResourceRecordType.MB;
                //case ResourceRecordType.MG: return ResourceRecordType.MG;
                //case ResourceRecordType.MR: return ResourceRecordType.MR;
                //case ResourceRecordType.NULL: return ResourceRecordType.NULL;
                //case ResourceRecordType.WKS: return ResourceRecordType.WKS;
                //case ResourceRecordType.PTR: return ResourceRecordType.PTR;
                //case ResourceRecordType.HINFO: return ResourceRecordType.HINFO;
                case ResourceRecordType.MX: return new MxResourceRecord();
                //case ResourceRecordType.TXT: return ResourceRecordType.TXT;
                default:
                    throw new NotSupportedException($"unsupported resource record type {type}");
            }
        }

        public static TimeSpan ParseTimeSpan(string val) {
            var res = TimeSpan.Zero;
            int? part = null;
            foreach (var ch in val) {
                if (ch >= '0' && ch <= '9') {
                    part = (part ?? 0) * 10 + (ch - '0');
                } else {
                    if (part == null) throw new FormatException("timespan value expected");
                    switch (char.ToLowerInvariant(ch)) {
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
                        default:
                            throw new NotSupportedException($"timespan unit {ch} is not supported");
                    }
                    part = null;
                }
            }
            if (part != null) {
                res += TimeSpan.FromSeconds(part.Value);
            }
            return res;
        }

        public static bool TryParseTimeSpan(string val, out TimeSpan timestamp) {
            timestamp = TimeSpan.Zero;
            int? part = null;
            foreach (var ch in val) {
                if (ch >= '0' && ch <= '9') {
                    part = (part ?? 0) * 10 + (ch - '0');
                } else {
                    if (part == null) return false;
                    switch (char.ToUpper(ch)) {
                        case 'w':
                            timestamp += TimeSpan.FromDays(part.Value * 7);
                            break;
                        case 'd':
                            timestamp += TimeSpan.FromDays(part.Value);
                            break;
                        case 'h':
                            timestamp += TimeSpan.FromHours(part.Value);
                            break;
                        case 'm':
                            timestamp += TimeSpan.FromMinutes(part.Value);
                            break;
                        case 's':
                            timestamp += TimeSpan.FromSeconds(part.Value);
                            break;
                        default:
                            return false;
                    }
                    part = null;
                }
            }
            if (part != null) {
                timestamp += TimeSpan.FromSeconds(part.Value);
            }
            return true;
        }


    }

}
