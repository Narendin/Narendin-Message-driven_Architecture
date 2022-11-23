using Messaging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SettingRabbit;
using System.Net.Security;
using System.Text;

namespace Restourant.Notifications_V2
{
    class ReceiveLogs
    {
        public static void Main()
        {
            using (var connection = new Consumer().ConnectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: Setting.Exchange, type: ExchangeType.Fanout);

                var queueName = channel.QueueDeclare().QueueName;
                channel.QueueBind(queue: queueName,
                                  exchange: Setting.Exchange,
                                  routingKey: "");

                Console.WriteLine("Waiting for logs.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine("{0}", message);
                };
                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);
                Console.ReadLine();
            }
        }
    }
}