using System;
using System.Threading.Tasks;

namespace EmailWorker.Services
{
    public interface IEmailService : IDisposable
    {
        Task SendEmailAsync(string receiverEmail, string subject, string message);
        void ConsumeMessages();
    }
}