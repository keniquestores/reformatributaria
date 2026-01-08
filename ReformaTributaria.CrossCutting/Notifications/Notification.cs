namespace ReformaTributaria.CrossCutting.Notifications
{
    public class Notification(string message, NotificationType type)
    {
        public string Message { get; } = message;

        public NotificationType Type { get; } = type;
    }
}
