using NotificationService.Data;
using NotificationService.Models;

public class NotificationServiceImpl : INotificationService
{
    private readonly NotificationDbContext _context;
    private readonly IRabbitMqHelper _rabbitMq;

    public NotificationServiceImpl(NotificationDbContext context, IRabbitMqHelper rabbitMq)
    {
        _context = context;
        _rabbitMq = rabbitMq;
    }

    public async Task<Notification> CreateNotificationAsync(Notification notification)
    {
        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

        // Send the notification to RabbitMQ exchange
        _rabbitMq.SendMessageToExchange("notifications", "new.notification", notification);

        return notification;
    }
}