namespace Les1
{
    public class Notification
    {
        /// <summary>
        /// Задержка сообщения
        /// </summary>
        public int SendDelay { get; init; }

        /// <summary>
        /// Выслать уведомление
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <returns></returns>
        public async Task Send(string message)
        {
            await Task.Delay(SendDelay);
            Console.WriteLine("Уведомление: " + message);
        }
    }
}