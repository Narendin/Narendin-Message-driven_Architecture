using System.Threading.Tasks;
using MassTransit;
using Restaurant.Messages;

namespace Restaurant.Kitchen.Consumers
{
    public class KitchenBookingRequestedConsumer : IConsumer<ITableBooked>
    {
        private readonly Manager _manager;

        public KitchenBookingRequestedConsumer(Manager manager)
        {
            _manager = manager;
        }

        public async Task Consume(ConsumeContext<ITableBooked> context)
        {
            var rnd = new Random().Next(1,10);
            if (rnd > 8)
            {
                throw new Exception("Пожар!");
            }

            if (_manager.CheckKitchenReady(context.Message.OrderId, context.Message.PreOrder))
                await context.Publish<IKitchenReady>(new KitchenReady(context.Message.OrderId));
        }
    }
}
