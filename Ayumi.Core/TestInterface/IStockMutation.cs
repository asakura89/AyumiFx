namespace TestInterface
{
    public interface IStockMutation
    {
        void Save(StockMutationData mutationData);
        void Delete(string mutationId);
        StockMutationData Get(string mutationId);
    }
}
