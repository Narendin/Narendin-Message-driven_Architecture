namespace Restaurant.Messages
{
    public interface IKitchenReady
    {
        public Guid OrderId { get; }

        public int NumberTable { get; }

        public bool Ready { get; }
    }

    public class KitchenReady : IKitchenReady
    {
        public KitchenReady(Guid orderId, int numberTable, bool ready)
        {
            OrderId = orderId;
            NumberTable = numberTable;
            Ready = ready;
        }

        public Guid OrderId { get; }

        public int NumberTable { get; }

        public bool Ready { get; }
    }
}
