using Messaging;

namespace Restaraunt.Booking
{
    public class Restaurant
    {
        private readonly List<Table> _tables = new();
        private readonly AutoResetEvent _event = new(true);
        private readonly PeriodicTimer _timer = new(TimeSpan.FromSeconds(100));
        private readonly CancellationTokenSource _freeTablesCancellationSource = new();
        private readonly Producer _producer = new Producer();

        public Restaurant()
        {
            for (byte i = 1; i <= 10; i++)
            {
                _tables.Add(new Table(i));
            }

            FreeTables(_freeTablesCancellationSource.Token);
        }

        async Task FreeTables(CancellationToken token)
        {
            while (await _timer.WaitForNextTickAsync(token))
            {
                if (token.IsCancellationRequested) return;

                var bookedTables = _tables.Where(t => t.State == StateTable.Booked).ToList();

                foreach (var table in bookedTables)
                    table.SetState(StateTable.Free);
                Console.WriteLine("Бронь всех столов сброшена");
            }
        }

        /// <summary>
        /// Показать список столов
        /// </summary>
        public void ShowTable()
        {
            foreach (var item in _tables)
            {
                Console.WriteLine(item);
            }
        }

        /// <summary>
        /// Бронирование стола 
        /// </summary>
        /// <param name="countOfPersons"></param>
        public void BookFreeTable(int countOfPersons)
        {
            _event.WaitOne();
            Console.WriteLine("Добрый день! Подождите секунду я подберу столик и подтвержу вашу бронь, оставайтесь на линии");

            var table = _tables.FirstOrDefault(t => t.SeatsCount >= countOfPersons && t.State == StateTable.Free);

            Thread.Sleep(1000 * 5);

            Console.WriteLine(table is null
                ? "К сожалению, сейчас все столики заняты"
                : "Готово! Ваш столик номер " + table.Id);
            _event.Set();
        }

        /// <summary>
        /// Бронирование стола (Async)
        /// </summary>
        /// <param name="countOfPersons"></param>
        public void BookFreeTableAsync(int countOfPersons)
        {
            _event.WaitOne();
            Console.WriteLine("Добрый день! Подождите секунду я подберу столик и подтвержу вашу бронь, вам придёт уведомление");

            Task.Run(async () =>
            {

                var table = _tables.FirstOrDefault(t => t.SeatsCount >= countOfPersons && t.State == StateTable.Free);

                await Task.Delay(1000 * 5);
                table?.SetState(StateTable.Booked);


                _producer.SendToQueue("Restaurant", table is null
                ? "К сожалению, сейчас все столики заняты"
                : "Готово! Ваш столик номер " + table.Id);
                _event.Set();
            });
        }

        /// <summary>
        /// Отмена брони стола
        /// </summary>
        /// <param name="id">Id Стола</param>
        public void FreeTable(int id)
        {
            _event.WaitOne();
            Console.WriteLine("Добрый день! Подождите секунду я освобожу столик, оставайтесь на линии");
            var table = _tables.FirstOrDefault(t => t.Id == id);

            Thread.Sleep(1000 * 5);

            table?.SetState(StateTable.Free);

            Console.WriteLine(table is null
                ? "Такого столика нет в нашем ресторане"
                : "Готово! Мы отменили вашу бронь");
            _event.Set();
        }

        /// <summary>
        /// Отмена брони стола (Async)
        /// </summary>
        /// <param name="id">Id Стола</param>
        public void FreeTableAsync(int id)
        {
            _event.WaitOne();
            Console.WriteLine("Добрый день! Подождите секунду я освобожу столик, вам придёт уведомление");

            Task.Run(async () =>
            {
                var table = _tables.FirstOrDefault(t => t.Id == id && t.State == StateTable.Booked);

                await Task.Delay(1000 * 5);

                table?.SetState(StateTable.Free);

                _producer.SendToQueue("Restaurant", table is null
                    ? "Такого столика нет в нашем ресторане"
                    : "Готово! Мы отменили вашу бронь" );
                _event.Set();
            });
        }
    }
}
