namespace NotificationService.Data.DTOs;

public class NotificationDTO
{
    public int NotificationId { get; set; }
    public Program.NotificationType NotificationType { get; set; }
    public string Source { get; set; }
    public string Data { get; set; }
    public int SubjectId { get; set; }
}