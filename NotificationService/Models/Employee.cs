namespace NotificationService.Models;

public class Employee
{
    public int EmployeeId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public int TeamId { get; set; } // Add this line
    public Team Team { get; set; }
    public ICollection<EmployeeTeam> EmployeeTeams { get; set; }
}
