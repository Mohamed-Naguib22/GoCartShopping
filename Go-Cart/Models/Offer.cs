namespace Go_Cart.Models
{
    public class Offer
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Product Product { get; set; }
    }
}
