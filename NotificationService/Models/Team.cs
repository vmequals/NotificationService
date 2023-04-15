namespace NotificationService.Models;

public class Team
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public ICollection<SubjectTeam> SubjectTeams { get; set; }
    public ICollection<EmployeeTeam> EmployeeTeams { get; set; }
}

