using DnsZone.Records;

namespace DnsZone.Formatter {
    public class ResourceRecordWriter : IResourceRecordVisitor<DnsZoneFormatterContext, ResourceRecord> {

        public ResourceRecord Visit(AResourceRecord record, DnsZoneFormatterContext context) {
            context.WriteIpAddress(record.Address);
            return record;
        }

        public ResourceRecord Visit(AaaaResourceRecord record, DnsZoneFormatterContext context) {
            context.WriteIpAddress(record.Address);
            return record;
        }

        public ResourceRecord Visit(CNameResourceRecord record, DnsZoneFormatterContext context) {
            context.WriteAndCompressDomainName(record.CanonicalName);
            return record;
        }

        public ResourceRecord Visit(MxResourceRecord record, DnsZoneFormatterContext context) {
            context.WritePreference(record.Preference);
            context.WriteAndCompressDomainName(record.Exchange);
            return record;
        }

        public ResourceRecord Visit(CAAResourceRecord record, DnsZoneFormatterContext context) {
            context.WritePreference(record.Flag);
            context.WriteTag(record.Tag);
            context.WriteString(record.Value);
            return record;
        }

        public ResourceRecord Visit(NsResourceRecord record, DnsZoneFormatterContext context) {
            context.WriteAndCompressDomainName(record.NameServer);
            return record;
        }

        public ResourceRecord Visit(PtrResourceRecord record, DnsZoneFormatterContext context) {
            context.WriteAndCompressDomainName(record.HostName);
            return record;
        }

        public ResourceRecord Visit(SoaResourceRecord record, DnsZoneFormatterContext context) {
            context.WriteAndCompressDomainName(record.NameServer);
            context.WriteEmail(record.ResponsibleEmail);
            context.WriteSerialNumber(record.SerialNumber);
            context.WriteTimeSpan(record.Refresh);
            context.WriteTimeSpan(record.Retry);
            context.WriteTimeSpan(record.Expiry);
            context.WriteTimeSpan(record.Minimum);
            return record;
        }

        public ResourceRecord Visit(SrvResourceRecord record, DnsZoneFormatterContext context) {
            context.WritePreference(record.Priority);
            context.WritePreference(record.Weight);
            context.WritePreference(record.Port);
            context.WriteAndCompressDomainName(record.Target);
            return record;
        }

        public ResourceRecord Visit(TxtResourceRecord record, DnsZoneFormatterContext context) {
            context.WriteString(record.Content);
            return record;
        }

        public ResourceRecord Visit(DNSKEYResourceRecord record, DnsZoneFormatterContext context)
        {
            context.WritePreference(record.Flag);
            context.WritePreference(record.Protocol);
            context.WriteString(record.Algorithm);
            context.WriteString(record.PublicKey);
            return record;
        }

        public ResourceRecord Visit(RRSIGResourceRecord record, DnsZoneFormatterContext context)
        {
            context.WriteString(record.TypeCovered);
            context.WritePreference(record.Algorithm);
            context.WriteString(record.Labels);
            context.WritePreference(record.OriginalTTL);
            context.WritePreference(record.SignatureExpiration);
            context.WritePreference(record.SignatureInception);
            context.WritePreference(record.KeyTag);
            context.WriteString(record.SignerName);
            context.WriteString(record.Signature);

            return record;
        }

        public ResourceRecord Visit(NSEC3ResourceRecord record, DnsZoneFormatterContext context)
        {
            context.WritePreference(record.HashAlgorithm);
            context.WritePreference(record.Flags);
            context.WritePreference(record.Iterations);
            context.WriteString(record.Salt);
            context.WriteString(record.NextHashedOrderName);
            context.WriteString(record.RecordTypes);

            return record;
        }

        public ResourceRecord Visit(NSEC3PARAMResourceRecord record, DnsZoneFormatterContext context)
        {
            context.WritePreference(record.HashAlgorithm);
            context.WritePreference(record.Flags);
            context.WritePreference(record.Iterations);
            context.WriteString(record.Salt);

            return record;
        }

        public ResourceRecord Visit(DSResourceRecord record, DnsZoneFormatterContext context)
        {
            context.WritePreference(record.KeyTag);
            context.WritePreference(record.Algorithm);
            context.WritePreference(record.DigestType);
            context.WriteString(record.Digest);

            return record;
        }
    }
}
