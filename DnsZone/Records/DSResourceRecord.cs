namespace DnsZone.Records {
    public class DSResourceRecord : ResourceRecord {
        
        public int KeyTag { get; set; }
        public int Algorithm { get; set; }
        public int DigestType { get; set; }

        public string Digest { get; set; }
        
        public override ResourceRecordType Type => ResourceRecordType.DS;
        
        public override TResult AcceptVistor<TArg, TResult>(IResourceRecordVisitor<TArg, TResult> visitor, TArg arg) {
            return visitor.Visit(this, arg);
        }

    }
}
