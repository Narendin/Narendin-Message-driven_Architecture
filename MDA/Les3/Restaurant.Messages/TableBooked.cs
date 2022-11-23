namespace Restaurant.Messages
{
    public interface ITableBooked
    {
        public Guid OrderId { get; }

        public Guid ClientId { get; }

        public Dish? PreOrder { get; }

        public Accepted Success { get; }

        public int NumberTable { get; }
    }

    public class TableBooked : ITableBooked
    {
        public TableBooked(Guid orderId, Guid clientId, Accepted success, int numberTable, Dish? preOrder = null)
        {
            OrderId = orderId;
            ClientId = clientId;
            Success = success;
            PreOrder = preOrder;
            NumberTable = numberTable;
        }

        public Guid OrderId { get; }
        public Guid ClientId { get; }
        public Dish? PreOrder { get; }
        public Accepted Success { get; }
        public int NumberTable { get; }
    }
}