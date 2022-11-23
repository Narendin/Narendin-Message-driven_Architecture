namespace Restaurant.Messages
{
    public interface ITableBooked
    {
        public Guid OrderId { get; }

        public Guid ClientId { get; }

        public bool Success { get; }

        public DateTime CreationDate { get; }

        public Dish? PreOrder { get; }
    }

    public class TableBooked : ITableBooked
    {
        public TableBooked(Guid orderId, Guid clientId, Dish? preOrder, bool success)
        {
            OrderId = orderId;
            ClientId = clientId;
            PreOrder = preOrder;
            Success = success;
        }

        public Guid OrderId { get; }

        public Guid ClientId { get; }
        public Dish? PreOrder { get; }
        public bool Success { get; }
        public DateTime CreationDate { get; }
    }
}