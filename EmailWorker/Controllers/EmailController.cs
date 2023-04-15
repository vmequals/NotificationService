using Microsoft.AspNetCore.Mvc;
using EmailWorker.Services;
using System.Threading.Tasks;
using EmailWorker.Data;
using EmailWorker.Models;

namespace EmailWorker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail(EmailDto emailDto)
        {
            // create email message
            var message = new EmailMessage
            {
                Recipient = emailDto.Recipient,
                Subject = emailDto.Subject,
                Body = emailDto.Body
            };

            // send email
            await _emailService.SendEmailAsync(message.Recipient, message.Subject, message.Body);
            return Ok();

        }
    }
}