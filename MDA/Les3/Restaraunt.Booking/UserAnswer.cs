using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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