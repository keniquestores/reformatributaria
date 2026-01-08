using FluentValidation.Results;

namespace ReformaTributaria.CrossCutting.Notifications
{
    public class NotificationHandler : INotificationHandler
    {
        private List<Notification> _notifications;

        public NotificationHandler()
        {
            _notifications = [];
        }

        public IReadOnlyCollection<Notification> GetNotifications() => _notifications;

        public bool HasNotificationsErrors() => _notifications.Any(x => x.Type == NotificationType.Error);

        public void DisposeNotifications() => _notifications = [];

        public void AddNotification(string message, NotificationType type)
        {
            _notifications.Add(new Notification(message, type));
        }

        public void AddNotifications(ValidationResult validationResult, NotificationType type)
        {
            if (validationResult.Errors == null) return;

            foreach (var msgError in from error in validationResult.Errors let msgError = error.ErrorMessage select msgError)
            {
                AddNotification(msgError, type);
            }
        }
    }
}
