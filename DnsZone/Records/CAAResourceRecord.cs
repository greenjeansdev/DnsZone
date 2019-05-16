using System;
using System.Collections.Generic;
using System.Text;

namespace DnsZone.Records
{
    public class CAAResourceRecord:ResourceRecord
    {
        public ushort flag { get; set; }
        public string tag { get; set; }
        public string value { get; set; }
        public override ResourceRecordType Type => ResourceRecordType.CAA;
        public override TResult AcceptVistor<TArg, TResult>(IResourceRecordVisitor<TArg, TResult> visitor, TArg arg)
        {
            return visitor.Visit(this, arg);
        }
    }
}
