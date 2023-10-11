namespace Go_Cart.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime PlacedOn { get; set; }
        public bool IsPaid { get; set; }
        public decimal TotalCost { get; set; }
        public string Status { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Transaction Transaction { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
        public IEnumerable<OrderItem> ProductOrders { get; set; }
    }
}
