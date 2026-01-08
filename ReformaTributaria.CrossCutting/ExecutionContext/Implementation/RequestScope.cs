using ReformaTributaria.CrossCutting.ExecutionContext.Abstractions;
using ReformaTributaria.CrossCutting.Notifications;
using ReformaTributaria.CrossCutting.Validation;

namespace ReformaTributaria.CrossCutting.ExecutionContext.Implementation
{
    public sealed class RequestScope : IRequestScope
    {
        private readonly IEnumerable<ITransactionalContext> _contexts;
        private readonly INotificationHandler _notificationHandler;
        private readonly IValidatorHandler _validatorHandler;

        public RequestScope(
            IEnumerable<ITransactionalContext> contexts,
            INotificationHandler notificationHandler,
            IValidatorHandler validatorHandler)
        {
            _contexts = contexts;
            _notificationHandler = notificationHandler;
            _validatorHandler = validatorHandler;
        }

        public async Task BeginAsync()
        {
            foreach (var ctx in _contexts)
                await ctx.BeginAsync();
        }

        public async Task CompleteAsync()
        {
            foreach (var ctx in _contexts)
                await ctx.SaveAsync();

            foreach (var ctx in _contexts)
                await ctx.CommitAsync();

            _notificationHandler.DisposeNotifications();
        }

        public async Task FailAsync()
        {
            foreach (var ctx in _contexts)
                await ctx.RollbackAsync();
        }

        public bool ShouldCommit()
        {
            if (_notificationHandler.HasNotificationsErrors())
                return false;

            return _validatorHandler.Commit();
        }
    }

}
