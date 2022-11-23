using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SettingRabbit;
using System.Net.Security;
using System.Text;

namespace Messaging
{
    public class Consumer
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly IModel? _channel;
        private readonly string? _queueName;

        public ConnectionFactory ConnectionFactory  => _connectionFactory;
        public Consumer()
        {
            _connectionFactory = new ConnectionFactory
            {
                HostName = Setting.HostName,
                VirtualHost = Setting.VirtualHost,
                UserName = Setting.UserName,
                Password = Setting.Password,
                Port = Setting.Port,
                RequestedHeartbeat = TimeSpan.FromSeconds(10),
                Ssl = new SslOption
                {
                    Enabled = true,
                    AcceptablePolicyErrors = SslPolicyErrors.RemoteCertificateNameMismatch |
                                             SslPolicyErrors.RemoteCertificateChainErrors,
                    Version = System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls11
                }
            };
        }

        public Consumer(string queueName):base()
        {
            _channel = _connectionFactory.CreateConnection().CreateModel();
            _queueName = queueName;
        }
        
        /// <summary>
        /// Получить сообщение из очереди
        /// </summary>
        /// <param name="receiveCallback"></param>
        public void Receive(EventHandler<BasicDeliverEventArgs> receiveCallback)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += receiveCallback;

            _channel.BasicConsume(_queueName, true, consumer);
        }
    }
}
