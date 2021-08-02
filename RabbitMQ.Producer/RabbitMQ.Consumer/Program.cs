using RabbitMQ.Client;
using System;

namespace RabbitMQ.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            QueueConsumer.Consume(channel);
        }
    }
}
