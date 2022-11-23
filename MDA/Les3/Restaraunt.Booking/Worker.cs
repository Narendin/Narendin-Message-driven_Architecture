using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.Hosting;
using Restaurant.Messages;

namespace Restaurant.Booking
{
    public class Worker : BackgroundService
    {
        private readonly IBus _bus;
        private readonly Restaurant _restaurant;

        public Worker(IBus bus, Restaurant restaurant)
        {
            _bus = bus;
            //_bus.GetRabbitMqHostTopology();
            _restaurant = restaurant;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("Привет! Желаете забронировать или освободить столик?\n" +
                      "1 - забронировать, мы уведомим Вас по смс (асинхронно)\n" +
                      "2 - освободить, мы уведомим Вас по смс (асинхронно)\n" +
                      "3 - Показать все столы\n"
                      );

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Введите, пожалуйста от 1 до 3");
                    continue;
                }
                var dateTime = DateTime.Now;
                int tableId;
                switch (choice)
                {
                    case (int)UserAnswer.BookingAsync:
                        Console.WriteLine("Укажите номер столика для бронирования");
                        int.TryParse(Console.ReadLine(), out tableId);
                        Console.WriteLine("IBookingRequest");
                        await _bus.Publish(new BookingRequest(Guid.NewGuid(), Guid.NewGuid(), null, dateTime), stoppingToken);
                        break;
                    case (int)UserAnswer.CancelBookingAsync:
                        Console.WriteLine("Укажите номер столика");
                        int.TryParse(Console.ReadLine(), out tableId);
                        break;
                    case (int)UserAnswer.ShowAllTable:
                        _restaurant.ShowTable();
                        break;
                    default:
                        break;
                }

                Console.WriteLine("Спасибо за Ваше обращение!");
            }
        }
    }
}
