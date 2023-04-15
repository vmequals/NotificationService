using NotificationService.Data.DTOs;
using Microsoft.EntityFrameworkCore;
using NotificationService.Data;
using NotificationService.Mappers;
using NotificationService.Services;
using NotificationService.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;


namespace NotificationService
{
    public class Program
    {
        
        public enum NotificationType
        {
            Email,
            SMS,
            PushNotification
        }
        
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var dataService = scope.ServiceProvider.GetRequiredService<IDataService>();

                // Example: Create a new team
                TeamDTO teamDTO = new TeamDTO { Name = "Team A" };
                teamDTO = await dataService.CreateTeamAsync(teamDTO);
                Console.WriteLine($"Created team: {teamDTO.Name} with ID: {teamDTO.TeamId}");

                // Example: Create random data
                Random random = new Random();
                List<TeamDTO> teams = new List<TeamDTO>();
                List<EmployeeDTO> employees = new List<EmployeeDTO>();
                List<SubjectDTO> subjects = new List<SubjectDTO>();
                List<NotificationDTO> notifications = new List<NotificationDTO>();

                // Create 3 teams
                for (int i = 1; i <= 3; i++)
                {
                    var team = await dataService.CreateTeamAsync(new TeamDTO { Name = $"Team {i}" });
                    teams.Add(team);
                }

                // Create 6 employees
                for (int i = 1; i <= 6; i++)
                {
                    var employee = await dataService.CreateEmployeeAsync(new EmployeeDTO
                    {
                        FirstName = $"First {i}",
                        LastName = $"Last {i}",
                        Email = $"first{i}.last{i}@example.com",
                        TeamId = teams[random.Next(teams.Count)].TeamId
                    });
                    employees.Add(employee);
                }

                // Create 4 subjects
                for (int i = 1; i <= 4; i++)
                {
                    var subject = await dataService.CreateSubjectAsync(new SubjectDTO { Name = $"Subject {i}", TeamId = teams[random.Next(teams.Count)].TeamId });
                    subjects.Add(subject);
                }

                // Create 10 notifications
                for (int i = 1; i <= 10; i++)
                {
                    var notificationTypeValues = Enum.GetValues(typeof(NotificationType));
                    var randomNotificationType = (NotificationType)notificationTypeValues.GetValue(random.Next(notificationTypeValues.Length));

                    var notification = await dataService.CreateNotificationAsync(new NotificationDTO
                    {
                        NotificationType = randomNotificationType,
                        Source = $"Source {i}",
                        Data = $"{{\"key\": \"value{i}\"}}",
                        SubjectId = subjects[random.Next(subjects.Count)].SubjectId
                    });
                    notifications.Add(notification);
                }

            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IDataService, DataService>();
                    services.AddDbContext<NotificationDbContext>(options =>
                        options.UseNpgsql("Postgres"));
                    services.AddAutoMapper(typeof(MappingProfile));
                });
    }

}