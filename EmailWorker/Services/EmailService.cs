using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using EmailWorker.Models;

namespace EmailWorker.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly IConnection _rabbitMQConnection;

        public EmailService(IConfiguration config)
        {
            _config = config;

            var rabbitMQConnectionFactory = new ConnectionFactory()
            {
                HostName = _config["RabbitMQ:HostName"],
                UserName = _config["RabbitMQ:UserName"],
                Password = _config["RabbitMQ:Password"]
            };

            _rabbitMQConnection = rabbitMQConnectionFactory.CreateConnection();
        }

        public async Task SendEmailAsync(string receiverEmail, string subject, string message)
        {
            try
            {
                var smtpClient = new SmtpClient(_config["Email:SmtpServer"], int.Parse(_config["Email:SmtpPort"]))
                {
                    Credentials = new NetworkCredential(_config["Email:Username"], _config["Email:Password"]),
                    EnableSsl = bool.Parse(_config["Email:EnableSsl"])
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_config["Email:FromAddress"]),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(receiverEmail);

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }

        public void ConsumeMessages()
        {
            using (var channel = _rabbitMQConnection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "notifications", type: ExchangeType.Fanout);

                var queueName = channel.QueueDeclare().QueueName;
                channel.QueueBind(queue: queueName, exchange: "notifications", routingKey: "");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($"Received message: {message}");

                    var messageParts = message.Split(',');
                    var receiverEmail = messageParts[0];
                    var subject = messageParts[1];
                    var emailMessage = messageParts[2];

                    SendEmailAsync(receiverEmail, subject, emailMessage).Wait();
                };

                channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

                Console.WriteLine("Email service listening for messages...");
                Console.ReadLine();
            }
        }

        public void Dispose()
        {
            _rabbitMQConnection.Dispose();
        }
    }
}
