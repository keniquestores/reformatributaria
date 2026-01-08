namespace ReformaTributaria.CrossCutting.ExecutionContext.Abstractions
{
    public interface ITransactionalContext
    {
        Task BeginAsync();
        Task SaveAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
