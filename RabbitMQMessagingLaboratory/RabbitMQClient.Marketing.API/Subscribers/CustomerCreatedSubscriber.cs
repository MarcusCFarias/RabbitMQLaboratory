
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Channels;

namespace RabbitMQClient.Marketing.API.Subscribers
{
    public class CustomerCreatedSubscriber : IHostedService
    {
        private const string EXCHANGE = "testing-rabbitmq";
        private const string QUEUE = "customer-created";
        private readonly IModel _channel;
        public CustomerCreatedSubscriber()
        {
            var connectionFactory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            var connection = connectionFactory.CreateConnection("subscriber-marketing-connection");
            _channel = connection.CreateModel();

            _channel.ExchangeDeclare(exchange: EXCHANGE, type: "topic", durable: false, autoDelete: false);
            _channel.QueueDeclare(queue: QUEUE, durable: false, exclusive: false, autoDelete: false);
            _channel.QueueBind(queue: QUEUE, exchange: EXCHANGE, routingKey: QUEUE);
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (sender, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine($"[CustomerCreatedSubscriber] - {message}");

                _channel.BasicAck(eventArgs.DeliveryTag, false);
            };

            _channel.BasicConsume(QUEUE, false, consumer);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Closing service stopped.");

            return Task.CompletedTask;
        }
    }
}
