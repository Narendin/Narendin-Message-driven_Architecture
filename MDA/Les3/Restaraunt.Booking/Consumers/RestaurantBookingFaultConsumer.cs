using MassTransit;
using Restaurant.Messages;

namespace Restaurant.Booking.Consumers
{
    public class RestaurantBookingFaultConsumer : IConsumer<Fault<IBookingRequest>>
    {
        private readonly Restaurant _restaurant;

        public RestaurantBookingFaultConsumer(Restaurant restaurant)
        {
            _restaurant = restaurant;
        }

        public Task Consume(ConsumeContext<Fault<IBookingRequest>> context)
        {
            Console.WriteLine($"[OrderId: {context.Message.Message.OrderId}] Кухня сгорела!");
            var result = _restaurant.FreeTableAsync(1);

            return Task.CompletedTask;
        }
    }
}