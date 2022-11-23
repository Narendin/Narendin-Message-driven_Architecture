using System.Text;
using System.Diagnostics;

namespace Restaraunt.Booking
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            var rest = new Restaurant();
            int tableId;

            while (true)
            {
                await Task.Delay(10000);

                Console.WriteLine("Привет! Желаете забронировать или освободить столик?\n" +
                                  "1 - забронировать, мы уведомим Вас по смс (асинхронно)\n" +
                                  "2 - забронировать, подождите на линии, мы Вас оповестим (синхронно)\n" +
                                  "3 - освободить, мы уведомим Вас по смс (асинхронно)\n" +
                                  "4 - освободить, подождите на линии, мы Вас оповестим (синхронно)\n" +
                                  "5 - Показать все столы\n"
                                  );

                if (!int.TryParse(Console.ReadLine(), out int choice) && choice is not (1 or 2))
                {
                    Console.WriteLine("Введите, пожалуйста от 1 до 5");
                    continue;
                }

                var stopWatch = new Stopwatch();
                stopWatch.Start();

                switch (choice)
                {
                    case (int)UserAnswer.BookingAsync:
                        rest.BookFreeTableAsync(1);
                        break;
                    case (int)UserAnswer.BookingSync:
                        rest.BookFreeTable(1);
                        break;
                    case (int)UserAnswer.CancelBookingAsync:
                        Console.WriteLine("Укажите номер столика");
                        int.TryParse(Console.ReadLine(), out tableId);
                        rest.FreeTableAsync(tableId);
                        break;
                    case (int)UserAnswer.CancelBookingSync:
                        Console.WriteLine("Укажите номер столика");
                        int.TryParse(Console.ReadLine(), out tableId);
                        rest.FreeTable(tableId);
                        break;
                    case (int)UserAnswer.ShowAllTable:
                        rest.ShowTable();
                        break;
                    default:
                        break;
                }

                Console.WriteLine("Спасибо за Ваше обращение!");
                stopWatch.Stop();
                var ts = stopWatch.Elapsed;
                Console.WriteLine($"{ts.Seconds:80}:{ts.Milliseconds:00}");
            }
        }
    }
}