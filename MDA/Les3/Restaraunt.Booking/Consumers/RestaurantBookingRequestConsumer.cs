using MassTransit;
using Restaurant.Messages;

namespace Restaurant.Booking.Consumers
{
    public class RestaurantBookingRequestConsumer : IConsumer<IBookingRequest>
    {
        private readonly Restaurant _restaurant;

        public RestaurantBookingRequestConsumer(Restaurant restaurant)
        {
            _restaurant = restaurant;
        }

        public async Task Consume(ConsumeContext<IBookingRequest> context)
        {
            var rnd = new Random().Next(1, 10);
            if (rnd > 8)
            {
                throw new Exception("Ошибка!");
            }

            Console.WriteLine($"[OrderId: {context.Message.OrderId}]");
            var result = await _restaurant.BookFreeTableAsync(1);

            await context.Publish<ITableBooked>(new TableBooked(context.Message.OrderId, context.Message.ClientId, context.Message.PreOrder, result ?? false));
        }
    }
}