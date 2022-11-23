using Restaurant.Messages;

namespace Restaurant.Booking
{
    public class Restaurant
    {
        private readonly List<Table> _tables = new();
        private readonly PeriodicTimer _timer = new(TimeSpan.FromSeconds(50));
        private readonly CancellationTokenSource _freeTablesCancellationSource = new();

        public Restaurant()
        {
            for (byte i = 1; i <= 10; i++)
            {
                _tables.Add(new Table(i));
            }
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
        /// Найти свободный стол
        /// </summary>
        /// <param name="countOfPersons">Количество персон</param>
        /// <returns></returns>
        Table? FindFreeTable(int countOfPersons)
        {
            return _tables.FirstOrDefault(t => t.SeatsCount >= countOfPersons && t.State == StateTable.Free);
        }

        /// <summary>
        /// Найти стол по его Id
        /// </summary>
        /// <param name="id">Id стола</param>
        /// <param name="stateTable">Статус стола</param>
        /// <returns></returns>
        Table? FindTable(int id, StateTable stateTable)
        {
            return _tables.FirstOrDefault(t => t.Id == id && t.State == stateTable);
        }

        /// <summary>
        /// Бронирование стола (Async)
        /// </summary>
        /// <param name="countOfPersons"></param>
        public async Task<bool?> BookFreeTableAsync(int numberTable)
        {
            Console.WriteLine("Добрый день! Подождите секунду я подберу столик и подтвержу вашу бронь, вам придёт уведомление");

            var table = FindTable(numberTable, StateTable.Free);

            return table?.SetState(StateTable.Booked);
        }

        /// <summary>
        /// Отмена брони стола (Async)
        /// </summary>
        /// <param name="id">Id Стола</param>
        public async Task<bool?> FreeTableAsync(int id)
        {
            Console.WriteLine("Добрый день! Подождите секунду я освобожу столик, вам придёт уведомление");

            var table = FindTable(id, StateTable.Booked);

            return table?.SetState(StateTable.Free);
        }
    }
}
