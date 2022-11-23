namespace Restaurant.Booking
{
    public enum UserAnswer
    {
        /// <summary>
        /// Забронировать стол (асинхронно)
        /// </summary>
        BookingAsync = 1,

        /// <summary>
        /// Освободить стол (асинхронно)
        /// </summary>
        CancelBookingAsync = 2,

        /// <summary>
        /// Показать все столы
        /// </summary>
        ShowAllTable = 3
    }
}