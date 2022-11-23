using Messaging;
using Microsoft.Extensions.Hosting;
using System.Text;

namespace Restourant.Notifications
{
    public class Worker : BackgroundService
    {
        private readonly Consumer _consumer;
        public Worker()
        {
            _consumer = new Consumer("Restaurant");
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Receive((sender, args) =>
            {
                var body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Received {message}");
            });
        }
    }
}
