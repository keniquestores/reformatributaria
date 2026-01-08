namespace ReformaTributaria.CrossCutting.ExecutionContext.Abstractions
{
    public interface IRequestScope
    {
        Task BeginAsync();
        Task CompleteAsync();
        Task FailAsync();

        bool ShouldCommit();
    }

}
