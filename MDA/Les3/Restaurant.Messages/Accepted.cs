using System;

namespace Restaurant.Messages
{
    [Flags]
    public enum Accepted
    {
        Rejected,
        Kitchen,
        Booking,
        All = Kitchen | Booking,
        CancelBooking,
        TableNotFound,
        KitchenFire,
        KitchenStop = KitchenFire | Booking,
        TableIsBroken,
        TableIsBrokenKitchen = TableIsBroken | Booking
    }
}