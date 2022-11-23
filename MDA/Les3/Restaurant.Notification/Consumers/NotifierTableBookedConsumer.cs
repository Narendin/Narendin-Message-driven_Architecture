using System;
using System.Threading.Tasks;
using MassTransit;
using Restaurant.Messages;

namespace Restaurant.Notification.Consumers;

public class NotifierTableBookedConsumer : IConsumer<ITableBooked>
{
    private readonly Notifier _notifier;

    public NotifierTableBookedConsumer(Notifier notifier)
    {
        _notifier = notifier;
    }

    public Task Consume(ConsumeContext<ITableBooked> context)
    {
       _notifier.Accept(context.Message.OrderId, context.Message.Success, context.Message.NumberTable,
           context.Message.ClientId);

       return Task.CompletedTask;
    }
}