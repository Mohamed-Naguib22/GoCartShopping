namespace Go_Cart.Models
{
    public class Transaction
    {
        public string Id { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string BuyerId { get; set; }
        public DateTime PaymentDate { get; set; }
        public int OrderId { get; set; }
        public Order Oder { get; set; }
    }
}
