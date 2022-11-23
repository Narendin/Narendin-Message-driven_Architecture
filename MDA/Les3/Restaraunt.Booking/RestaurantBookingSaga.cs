using System;
using Automatonymous;
using MassTransit;
using Restaurant.Booking.Consumers;
using Restaurant.Messages;

namespace Restaurant.Booking
{
    public sealed class RestaurantBookingSaga : MassTransitStateMachine<RestaurantBooking>
    {
        public RestaurantBookingSaga()
        {
            InstanceState(x => x.CurrentState);

            Event(() => BookingRequested,
                x =>
                    x.CorrelateById(context => context.Message.OrderId)
                        .SelectId(context => context.Message.OrderId));
            
            Event(() => TableBooked,
                x =>
                    x.CorrelateById(context => context.Message.OrderId));

            Event(() => KitchenReady, 
                x =>
                    x.CorrelateById(context => context.Message.OrderId));

            CompositeEvent(() => BookingApproved, 
                x => x.ReadyEventStatus, KitchenReady, TableBooked);

            Event(() => BookingRequestFault,
                x =>
                    x.CorrelateById(m => m.Message.Message.OrderId));

            Initially(
                When(BookingRequested)
                    .Then(context =>
                    {
                        context.Saga.CorrelationId = context.Data.OrderId;
                        context.Saga.OrderId = context.Data.OrderId;
                        context.Saga.ClientId = context.Data.ClientId;
                        Console.WriteLine("Saga: " + context.Data.CreationDate);
                    })
                    .TransitionTo(AwaitingBookingApproved)
            );

            During(AwaitingBookingApproved,
                When(BookingApproved)
                    .Publish(context =>
                       (INotify) new Notify(
                           context.Saga.OrderId,
                           context.Saga.ClientId,
                           $"Стол успешно забронирован")) 
                    .Finalize(),
                When(BookingRequestFault)
                .Then(context => Console.WriteLine("Кухня сгорела!"))
                .Publish(context =>
                    (INotify)new Notify(
                        context.Saga.OrderId,
                        context.Saga.ClientId,
                        $"Приносим извинения, ресторан не работает."))
                .Finalize()
            );

            SetCompletedWhenFinalized();
        }
        public State AwaitingBookingApproved { get; private set; }
        public Event<IBookingRequest> BookingRequested { get; private set; }
        public Event<Fault<IBookingRequest>> BookingRequestFault { get; private set;  }
        public Event<ITableBooked> TableBooked { get; private set; }
        public Event<IKitchenReady> KitchenReady { get; private set; }
        public Event BookingApproved { get; private set;  }
    }
}