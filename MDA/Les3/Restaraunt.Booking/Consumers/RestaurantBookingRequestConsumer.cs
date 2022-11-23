using MassTransit;
using Microsoft.Extensions.Logging;
using Restaurant.Messages;
using Restaurant.Messages.InMemoryDb;

namespace Restaurant.Booking.Consumers
{
    public class RestaurantBookingRequestConsumer : IConsumer<IBookingRequest>
    {
        private readonly ILogger _logger;
        private readonly Restaurant _restaurant;
        private readonly IInMemoryRepository<BookingRequestModel> _repository;

        public RestaurantBookingRequestConsumer(Restaurant restaurant,
        IInMemoryRepository<BookingRequestModel> repository,
        ILogger<RestaurantBookingRequestConsumer> logger)
        {
            _restaurant = restaurant;
            _repository = repository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IBookingRequest> context)
        {
            _logger.Log(LogLevel.Information, $"[OrderId: {context.Message.OrderId}]");
            var rnd = new Random().Next(1, 10);
            if (rnd > 8)
            {
                throw new Exception("Ошибка!");
            }

            var model = _repository.Get().FirstOrDefault(i => i.OrderId == context.Message.OrderId);

            if (model is not null && model.CheckMessageId(context.MessageId.ToString()))
            {
                _logger.Log(LogLevel.Debug, context.MessageId.ToString());
                _logger.Log(LogLevel.Debug, "Second time");
                return;
            }

            var requestModel = new BookingRequestModel(context.Message.OrderId, context.Message.ClientId,
            context.Message.PreOrder, context.Message.CreationDate, context.MessageId.ToString());

            _logger.Log(LogLevel.Debug, context.MessageId.ToString());
            _logger.Log(LogLevel.Debug, "First time");
            var resultModel = model?.Update(requestModel, context.MessageId.ToString()) ?? requestModel;

            _repository.AddOrUpdate(resultModel);

            _logger.Log(LogLevel.Debug, $"[OrderId: {context.Message.OrderId}]");
            var result = await _restaurant.BookFreeTableAsync(1);

            await context.Publish<ITableBooked>(new TableBooked(context.Message.OrderId, context.Message.ClientId, context.Message.PreOrder, result ?? false));
        }
    }
}