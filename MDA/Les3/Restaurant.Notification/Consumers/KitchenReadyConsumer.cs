using System.Threading.Tasks;
using MassTransit;
using Restaurant.Messages;

namespace Restaurant.Notification.Consumers
{
    public class KitchenReadyConsumer : IConsumer<IKitchenReady>
    {

        private readonly Notifier _notifier;

        public KitchenReadyConsumer(Notifier notifier)
        {
            _notifier = notifier;
        }

        public Task Consume(ConsumeContext<IKitchenReady> context)
        {
            var rnd = new Random();

            if (context.Message.NumberTable == 5)
            {
                _notifier.Accept(context.Message.OrderId, Accepted.TableIsBroken);  // Почему то кухня знает что 5й стол сломан, а ресторан нет)
                return Task.CompletedTask;
            }

            if (rnd.Next(6) == 1)  //случайная поломка
            {
                _notifier.Accept(context.Message.OrderId, Accepted.KitchenFire);
            }
            else
            {
                _notifier.Accept(context.Message.OrderId, Accepted.Kitchen);
            }

            return Task.CompletedTask;
        }
    }
}
