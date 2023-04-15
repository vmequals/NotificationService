using System.Text;
using RabbitMQ.Client;
using Newtonsoft.Json;

namespace NotificationService.Helpers
{
    public class RabbitMqHelper : IRabbitMqHelper
    {
        private IConnection _connection;
        private IModel _channel;

        public RabbitMqHelper(string hostName)
        {
            var factory = new ConnectionFactory() { HostName = hostName };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void SendMessageToExchange<T>(string exchange, string routingKey, T message)
        {
            _channel.ExchangeDeclare(exchange, ExchangeType.Topic);

            var messageJson = JsonConvert.SerializeObject(message);
            var messageBody = Encoding.UTF8.GetBytes(messageJson);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.DeliveryMode = 2;

            _channel.BasicPublish(
                exchange: exchange,
                routingKey: routingKey,
                basicProperties: properties,
                body: messageBody);
        }

        public void Dispose()
        {
            _channel?.Close();
            _channel?.Dispose();
            _connection?.Close();
            _connection?.Dispose();
        }
    }
}