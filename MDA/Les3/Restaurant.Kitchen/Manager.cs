using MassTransit;
using Restaurant.Messages;

namespace Restaurant.Kitchen
{
    internal class Manager
    {
        private readonly IBus _bus;

        public Manager(IBus bus)
        {
            _bus = bus;
        }

        public void CheckKitchenReady(Guid orderId, int numberTable, Dish? dish)
        {
            _bus.Publish<IKitchenReady>(new KitchenReady(orderId, numberTable, true));
        }
    }
}
