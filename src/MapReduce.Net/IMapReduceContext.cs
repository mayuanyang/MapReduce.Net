namespace MapReduce.Net
{
    public interface IMapReduceContext<TKey, TValue>
    {
        void Save(TKey key, TValue value);
    }
}
