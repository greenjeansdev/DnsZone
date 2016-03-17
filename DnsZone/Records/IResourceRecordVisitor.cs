namespace DnsZone.Records {
    public interface IResourceRecordVisitor<TArg, TResult> {

        TResult Visit(AResourceRecord record, TArg arg);

        TResult Visit(AAAAResourceRecord record, TArg arg);

        TResult Visit(MxResourceRecord record, TArg arg);

        TResult Visit(NsResourceRecord record, TArg arg);

        TResult Visit(SoaResourceRecord record, TArg arg);

    }
}
