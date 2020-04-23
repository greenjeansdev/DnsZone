namespace DnsZone.Records {
    public class NSEC3PARAMResourceRecord : ResourceRecord {
        
        public int HashAlgorithm { get; set; }
        public int Flags { get; set; }
        public int Iterations { get; set; }

        public string Salt { get; set; }
       
        public override ResourceRecordType Type => ResourceRecordType.NSEC3;
        
        public override TResult AcceptVistor<TArg, TResult>(IResourceRecordVisitor<TArg, TResult> visitor, TArg arg) {
            return visitor.Visit(this, arg);
        }

    }
}
