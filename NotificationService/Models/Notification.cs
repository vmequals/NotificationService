namespace NotificationService.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public string NotificationType { get; set; }
        public string Source { get; set; }
        public string Data { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
    }


}