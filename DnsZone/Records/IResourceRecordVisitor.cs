namespace DnsZone.Records {
    public interface IResourceRecordVisitor<TArg, TResult> {

        TResult Visit(AResourceRecord record, TArg arg);

        TResult Visit(AaaaResourceRecord record, TArg arg);

        TResult Visit(CNameResourceRecord record, TArg arg);

        TResult Visit(MxResourceRecord record, TArg arg);

        TResult Visit(NsResourceRecord record, TArg arg);

        TResult Visit(PtrResourceRecord record, TArg arg);

        TResult Visit(SoaResourceRecord record, TArg arg);

        TResult Visit(SrvResourceRecord record, TArg arg);

        TResult Visit(TxtResourceRecord record, TArg arg);

        TResult Visit(CAAResourceRecord record, TArg arg);

        TResult Visit(DNSKEYResourceRecord record, TArg arg);

        TResult Visit(RRSIGResourceRecord record, TArg arg);
        TResult Visit(NSEC3ResourceRecord record, TArg arg);

        TResult Visit(NSEC3PARAMResourceRecord record, TArg arg);
        TResult Visit(DSResourceRecord record, TArg arg);
    }
}
