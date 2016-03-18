namespace DnsZone.Records {
    public class SrvResourceRecord : ResourceRecord {

        public ushort Priority { get; set; }

        public ushort Weight { get; set; }

        public ushort Port { get; set; }

        public string Target { get; set; }


        public override ResourceRecordType Type => ResourceRecordType.SRV;

        public override TResult AcceptVistor<TArg, TResult>(IResourceRecordVisitor<TArg, TResult> visitor, TArg arg) {
            return visitor.Visit(this, arg);
        }

    }
}
