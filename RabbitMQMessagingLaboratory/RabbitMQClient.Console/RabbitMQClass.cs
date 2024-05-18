using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RabbitMQClient.Console
{
    public class RabbitMQClass
    {
        private readonly IConnectionFactory _connectionFactory;
        public RabbitMQClass(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void Send<T>(string exchange, string routingKey, IBasicProperties basicProperties, T message)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                var channel = connection.CreateModel();

                var jsonMessage = System.Text.Json.JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(jsonMessage);
                channel.BasicPublish(exchange, routingKey, basicProperties, body);

                System.Console.WriteLine("Message sent to RabbitMQ. \n{0}", jsonMessage);
            }
        }

        public void Consume<T>(string queue)
        {
            var connection = _connectionFactory.CreateConnection();

            var channel = connection.CreateModel();

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (send, EventArgs) =>
            {
                var body = EventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                System.Console.WriteLine("Message received from RabbitMQ. \n{0}", message);

                channel.BasicAck(EventArgs.DeliveryTag, false);
            };

            channel.BasicConsume(queue, false, consumer);
        }
    }
}
