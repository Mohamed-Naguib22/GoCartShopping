namespace Go_Cart.Controllers
{
    internal class PaymentGateway
    {
        public bool Success { get; set; }
        public string TransactionId { get; set; }
        public PaymentGateway ProcessPayment(decimal totalCost)
        {
            return new PaymentGateway
            {
                Success = true,
                TransactionId = Guid.NewGuid().ToString()
            };
        }
    }
}