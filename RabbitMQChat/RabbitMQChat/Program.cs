using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQChat
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://chat:123@localhost:5672");

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            var exchangeName = "chat";
            var queueName = Guid.NewGuid().ToString();

            channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout);
            channel.QueueDeclare(queueName, true, true, true);
            channel.QueueBind(queueName, exchangeName, "");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, eventArgs) =>
            {
                var text = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
                Console.WriteLine(text);
            };

            channel.BasicConsume(queueName, true, consumer);

            var input = Console.ReadLine();
            while (input != "")
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                channel.BasicPublish(exchangeName, "", null, bytes);
                input = Console.ReadLine();
            }
            

            channel.Close();
            connection.Close();
        }
    }
}
