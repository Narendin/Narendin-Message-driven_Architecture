using MassTransit;
using Restaurant.Messages;

namespace Restaurant.Notification.Consumers
{
    public class KitchenReadyConsumer : IConsumer<INotify>
    {
        private readonly Notifier _notifier;

        public KitchenReadyConsumer(Notifier notifier)
        {
            _notifier = notifier;
        }

        public Task Consume(ConsumeContext<INotify> context)
        {
            var rnd = new Random();

            if (rnd.Next(6) == 1)
            {
                _notifier.Notify(context.Message.OrderId, context.Message.ClientId, context.Message.Message);
            }
            else
            {
                _notifier.Notify(context.Message.OrderId, context.Message.ClientId, context.Message.Message);
            }

            return Task.CompletedTask;
        }
    }
}