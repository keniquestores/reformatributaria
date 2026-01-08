using FluentValidation.Results;

namespace ReformaTributaria.CrossCutting.Notifications
{
    public interface INotificationHandler
    {
        void AddNotification(string message, NotificationType type);
        void AddNotifications(ValidationResult validationResult, NotificationType type);
        IReadOnlyCollection<Notification> GetNotifications();
        bool HasNotificationsErrors();
        void DisposeNotifications();
    }
}
