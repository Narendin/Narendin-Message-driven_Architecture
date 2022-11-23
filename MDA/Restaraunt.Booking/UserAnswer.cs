using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaraunt.Booking
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