namespace DnsZone.Records {
    public class RRSIGResourceRecord : ResourceRecord {
        
        public string TypeCovered { get; set; }
        public int Algorithm { get; set; }
        public string Labels { get; set; }
        public int OriginalTTL { get; set; }

        public long SignatureExpiration { get; set; }
        public long SignatureInception { get; set; }

        public int KeyTag { get; set; }
        public string SignerName { get; set; }

        public string Signature { get; set; }
        
        public override ResourceRecordType Type => ResourceRecordType.DNSKEY;
        
        public override TResult AcceptVistor<TArg, TResult>(IResourceRecordVisitor<TArg, TResult> visitor, TArg arg) {
            return visitor.Visit(this, arg);
        }

    }
}
