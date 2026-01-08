using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ReformaTributaria.CrossCutting.ExecutionContext.Exceptions;
using ReformaTributaria.CrossCutting.Notifications;

namespace ReformaTributaria.CrossCutting.Validation
{
    public class ValidatorHandler(INotificationHandler notificationHandler, IServiceProvider serviceProvider) : IValidatorHandler
    {
        private readonly INotificationHandler _notificationHandler = notificationHandler;
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private bool _commit = false;

        public void CommitChanges()
        {
            _commit = true;
        }

        public bool Commit() => _commit;

        public void AddMsgErrorAndStopExecution(string msg)
        {
            _notificationHandler.AddNotification(msg, NotificationType.Error);

            throw new ValidatorException();
        }

        public void AddMsgError(string msg)
        {
            _notificationHandler.AddNotification(msg, NotificationType.Error);
        }

        public void AddMsgInformation(string msg)
        {
            _notificationHandler.AddNotification(msg, NotificationType.Information);
        }

        public void ValidateAndAddMsgError<TEntity>(TEntity entity)
        {
            IServiceScope serviceScope = _serviceProvider.CreateScope();

            using var scope = serviceScope;

            var validatorService = scope.ServiceProvider.GetService<IValidator<TEntity>>();

            var validationresult = validatorService!.Validate(entity);

            if (!validationresult.IsValid)
                _notificationHandler.AddNotifications(validationresult, NotificationType.Error);
        }

        public void ValidateAndStopExecution<TEntity>(TEntity entity)
        {
            IServiceScope serviceScope = _serviceProvider.CreateScope();

            using var scope = serviceScope;

            var validatorService = scope.ServiceProvider.GetService<IValidator<TEntity>>();

            var validationresult = validatorService!.Validate(entity);

            if (!validationresult.IsValid)
            {
                _notificationHandler.AddNotifications(validationresult, NotificationType.Error);
                throw new ValidatorException();
            }
        }

        public bool Validate<TEntity>(TEntity entity)
        {
            IServiceScope serviceScope = _serviceProvider.CreateScope();

            using var scope = serviceScope;

            var validatorService = scope.ServiceProvider.GetService<IValidator<TEntity>>();

            var validationresult = validatorService!.Validate(entity);

            return validationresult.IsValid;
        }
    }
}
