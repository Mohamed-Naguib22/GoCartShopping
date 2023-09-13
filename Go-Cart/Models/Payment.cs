namespace Go_Cart.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public int OrderId { get; set; }
        public Order Oder { get; set; }
    }
}
