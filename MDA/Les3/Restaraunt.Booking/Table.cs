namespace Restaurant.Booking
{
    public class Table
    {
        private readonly object _lock = new();
        public StateTable State { get; private set; }
        public int SeatsCount { get; }
        public int Id { get; }

        public Table(int id)
        {
            Id = id;
            State = StateTable.Free;
            SeatsCount = Random.Shared.Next(2, 5);
        }

        /// <summary>
        /// Статсу стола
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool SetState(StateTable state)
        {
            lock (_lock)
            {
                if (state == State) return false;

                State = state;
                return true;
            }
        }

        public override string ToString()
        {
            return $"Стол №{Id} " + (State == StateTable.Free ? "Свободен" : "Забронирован");
        }
    }
}
