using NotificationService.Models;
using System.Threading.Tasks;

public interface INotificationService
{
    Task<Notification> CreateNotificationAsync(Notification notification);
}