using ExploreCalifornia.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using RabbitMQ.Client;
using System.Text;

namespace ExploreCalifornia.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var tourName = "a";
            var name = "Mohamad Reza Safari";
            var email = "email@gmail.com";
            var needsTransport = "transport on";

            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://guest:guest@localhost:5672");

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.ExchangeDeclare("webappExchange", ExchangeType.Fanout, true);

            var message = $"{tourName};{name};{email}";
            var bytes = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish("webappExchange", "", null, bytes);

            channel.Close();
            connection.Close();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
