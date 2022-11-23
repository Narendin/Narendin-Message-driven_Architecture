namespace Les1
{
    public enum UserAnswer
    {
        /// <summary>
        /// Забронировать стол (асинхронно)
        /// </summary>
        BookingAsync = 1,

        /// <summary>
        /// Забронировать стол (синхронно)
        /// </summary>
        BookingSync = 2,

        /// <summary>
        /// Освободить стол (асинхронно)
        /// </summary>
        CancelBookingAsync = 3,

        /// <summary>
        /// Освободить стол (синхронно)
        /// </summary>
        CancelBookingSync = 4,

        /// <summary>
        /// Показать все столы
        /// </summary>
        ShowAllTable = 5
    }
}