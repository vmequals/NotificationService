using System.Text;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NotificationConsumer.Configurations;
using NotificationConsumer.Hubs;
using NotificationConsumer.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NotificationConsumer.Services;

    public class NotificationConsumerService : BackgroundService
    {
        private readonly IHubContext<NotificationsHub> _hubContext;
        private readonly RabbitMQConfiguration _rabbitMqConfiguration;
        private readonly ILogger<NotificationConsumerService> _logger;

        public NotificationConsumerService(IHubContext<NotificationsHub> hubContext, IOptions<RabbitMQConfiguration> rabbitMQConfiguration, ILogger<NotificationConsumerService> logger)
        {
            _hubContext = hubContext;
            _rabbitMqConfiguration = rabbitMQConfiguration.Value;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory() { HostName = _rabbitMqConfiguration.Hostname };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: _rabbitMqConfiguration.QueueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var notification = JsonConvert.DeserializeObject<Notification>(message);

                // Send the notification to the Angular front end using SignalR
                await _hubContext.Clients.All.SendAsync("ReceiveNotification", notification);

                _logger.LogInformation($"Received notification: {message}");
            };

            channel.BasicConsume(
                queue: _rabbitMqConfiguration.QueueName,
                autoAck: true,
                consumer: consumer
            );

            return Task.CompletedTask;
        }
    }