using Restaurant.Messages;
using System.Collections.Concurrent;

namespace Restaurant.Notification
{
    public class Notifier
    {
        //импровизированный кэш для хранения статусов, номера заказа и клиента
        private readonly ConcurrentDictionary<Guid, Tuple<Guid?, Accepted, int?>> _state = new ();
        
        public void Accept(Guid orderId, Accepted accepted, int? numberTable = null, Guid? clientId = null)
        {
            _state.AddOrUpdate(orderId, new Tuple<Guid?, Accepted, int?>(clientId, accepted, numberTable),
                (guid, oldValue) => new Tuple<Guid?, Accepted, int?>(
                    oldValue.Item1 ?? clientId, oldValue.Item2 | accepted, oldValue.Item3 ?? numberTable));
            
            Notify(orderId);
        }

        private void Notify(Guid orderId)
        {
            var booking = _state[orderId];
            
            switch (booking.Item2)
            {
                case Accepted.All:
                    Console.WriteLine($"Стол № {booking.Item3} успешно забронирован для клиента {booking.Item1}");
                    _state.Remove(orderId, out _);
                    break;
                case Accepted.Rejected:
                    Console.WriteLine($"Гость {booking.Item1}, к сожалению, стол № {booking.Item3} занят");
                    _state.Remove(orderId, out _);
                    break;
                case Accepted.CancelBooking:
                    Console.WriteLine($"Гость {booking.Item1}, бронь Вашего стола № {booking.Item3} снята");
                    _state.Remove(orderId, out _);
                    break;
                case Accepted.KitchenStop:
                    Console.WriteLine($"Гость {booking.Item1}, к сожалению, кухня сгорела");
                    _state.Remove(orderId, out _);
                    break;
                case Accepted.TableIsBrokenKitchen:
                    Console.WriteLine($"Гость {booking.Item1}, у выбранного вами стола № {booking.Item3} сломана ножка");
                    _state.Remove(orderId, out _);
                    break;
                case Accepted.Kitchen:
                case Accepted.Booking:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}