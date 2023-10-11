namespace Go_Cart.Models
{
    public class OrderItem
    {
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public int Quantity { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public decimal SubTotalPrice { get; set; }
        public Product Product { get; set; }
        public Order Order { get; set; }
    }
}