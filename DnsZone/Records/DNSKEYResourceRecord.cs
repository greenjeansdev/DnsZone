namespace DnsZone.Records {
    public class DNSKEYResourceRecord : ResourceRecord {
        
        public ushort Flag { get; set; }
        
        public int Protocol { get; set; }
        
        public string Algorithm { get; set; }

        public string PublicKey { get; set; }
        
        public override ResourceRecordType Type => ResourceRecordType.DNSKEY;
        
        public override TResult AcceptVistor<TArg, TResult>(IResourceRecordVisitor<TArg, TResult> visitor, TArg arg) {
            return visitor.Visit(this, arg);
        }

    }
}
