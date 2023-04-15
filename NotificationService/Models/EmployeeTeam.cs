namespace NotificationService.Models;

public class EmployeeTeam
{
    public int TeamId { get; set; }
    public Team Team { get; set; }
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; }
}