using System;

namespace DnsZone.Records {
    public abstract class ResourceRecord {

        public string Name { get; set; }

        public string Class { get; set; }

        public abstract ResourceRecordType Type { get; }

        public TimeSpan Ttl { get; set; }

    }
}
