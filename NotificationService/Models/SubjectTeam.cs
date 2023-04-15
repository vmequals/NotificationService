namespace NotificationService.Models;

public class SubjectTeam
{
    public int SubjectId { get; set; }
    public Subject Subject { get; set; }
    public int TeamId { get; set; }
    public Team Team { get; set; }
}
