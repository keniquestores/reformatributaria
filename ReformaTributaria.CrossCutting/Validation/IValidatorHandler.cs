namespace ReformaTributaria.CrossCutting.Validation
{
    public interface IValidatorHandler
    {
        void AddMsgErrorAndStopExecution(string msg);
        void AddMsgError(string msg);
        void AddMsgInformation(string msg);
        void ValidateAndAddMsgError<TEntity>(TEntity entity);
        void ValidateAndStopExecution<TEntity>(TEntity entity);
        bool Validate<TEntity>(TEntity entity);
        void CommitChanges();
        bool Commit();
    }
}
