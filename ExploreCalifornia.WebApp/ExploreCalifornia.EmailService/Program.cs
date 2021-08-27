using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace ExploreCalifornia.EmailService
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://guest:guest@localhost:5672");

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare("emailServiceQueue", true, false, false);
            channel.QueueBind("emailServiceQueue", "webappExchange", "");

            var cosumer = new EventingBasicConsumer(channel);
            cosumer.Received += (sender, eventArgs) =>
            {
                var msg = Encoding.UTF8.GetString(eventArgs.Body.Span);
                Console.WriteLine(msg);
            };

            channel.BasicConsume("emailServiceQueue", true, cosumer);
            Console.ReadLine();

            channel.Close();
            connection.Close();
        }
    }
}
