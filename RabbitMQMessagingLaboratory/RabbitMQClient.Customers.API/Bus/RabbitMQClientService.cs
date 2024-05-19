using RabbitMQ.Client;
using System.Text.Json;

namespace RabbitMQClient.Customers.API.Bus
{
    public class RabbitMQClientService : IBusService
    {
        private readonly IModel _channel;
        private const string EXCHANGE = "testing-rabbitmq";
        private const string QUEUE = "customer-created";

        public RabbitMQClientService()
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest"
            };

            var connection = connectionFactory.CreateConnection("publisher-customers-connection");
            _channel = connection.CreateModel();

            _channel.ExchangeDeclare(exchange: EXCHANGE, type: "topic", durable: false, autoDelete: false);
            _channel.QueueDeclare(queue: QUEUE, durable: false, exclusive: false, autoDelete: false);
            _channel.QueueBind(queue: QUEUE, exchange: EXCHANGE, routingKey: QUEUE);
        }
        public void Publish<T>(string routingKey, T message)
        {
            var json = JsonSerializer.Serialize(message);
            var body = System.Text.Encoding.UTF8.GetBytes(json);
            _channel.BasicPublish(EXCHANGE, routingKey, null, body);
        }
    }
}
