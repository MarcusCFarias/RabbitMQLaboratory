using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQClient.Console;
using System.Text;
using System.Text.Json;

namespace RabbitMQClientConsole
{
    internal class Program
    {

        static void Main(string[] args)
        {
            var person = new Person("Marcus", "12345678911", new DateTime(1999, 7, 9));

            var connectionFactory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest"
            };

            var rabbitMQ = new RabbitMQClass(connectionFactory);
            rabbitMQ.Send(Exchange.TESTING, "hr.person-created", null, person);
            rabbitMQ.Consume<Person>("person-created");

            Console.ReadLine();
        }
    }
}
