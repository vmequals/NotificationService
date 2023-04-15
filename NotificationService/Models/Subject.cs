namespace NotificationService.Models;

public class Subject
{
    public int SubjectId { get; set; }
    public string Name { get; set; }
    public ICollection<SubjectTeam> SubjectTeams { get; set; } = new List<SubjectTeam>();
}