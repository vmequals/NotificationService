public interface IRabbitMqHelper : IDisposable
{
    void SendMessageToExchange<T>(string exchange, string routingKey, T message);
}
