// using Microsoft.AspNetCore.Mvc;
// using NotificationService.Data;
// using NotificationService.Models;
//
// namespace NotificationService.Controllers
// {
//     [ApiController]
//     [Route("api/notifications")]
//     public class NotificationsController : ControllerBase
//     {
//         private readonly NotificationDbContext _context;
//         private readonly IRabbitMqHelper _rabbitMq;
//
//         public NotificationsController(NotificationDbContext context, IRabbitMqHelper rabbitMq
//         )
//         {
//             _context = context;
//             _rabbitMq = rabbitMq;
//         }
//
//         [HttpPost]
//         public async Task<IActionResult> CreateNotification([FromBody] Notification notification)
//         {
//             if (notification == null)
//             {
//                 return BadRequest("Invalid notification data");
//             }
//             var createdNotification = new Notification
//             {
//                 Title = notification.Title,
//                 Message = notification.Message,
//                 Status = "unread",
//                 CreatedAt  = DateTime.UtcNow
//             };
//             
//             await _context.Notifications.AddAsync(notification);
//             await _context.SaveChangesAsync();
//             
//             // var responsibleUserOrGroup = await _oracleDbService.GetUserOrUserGroupForSubjectAsync(notificationDto.Subject);
//             // if (!string.IsNullOrEmpty(responsibleUserOrGroup))
//             // {
//             //     createdNotification.ResponsibleUserOrGroup = responsibleUserOrGroup;
//             //     await _dbContext.SaveChangesAsync();
//             // }
//             
//             // Send the notification to RabbitMQ exchange
//             _rabbitMq.SendMessageToExchange("notifications", "new.notification", createdNotification);
//
//             return Created($"/api/notifications/{notification.Id}", notification);
//         }
//         
//         [HttpPut("{id}/status")]
//         public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
//         {
//             var notification = await _context.Notifications.FindAsync(id);
//
//             if (notification == null)
//             {
//                 return NotFound("Notification not found");
//             }
//
//             notification.Status = status;
//             _context.Notifications.Update(notification);
//             await _context.SaveChangesAsync();
//
//             return NoContent();
//         }
//     }
// }