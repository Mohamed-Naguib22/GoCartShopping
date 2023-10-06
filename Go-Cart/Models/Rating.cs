namespace Go_Cart.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ApplicationUserId { get; set; }
        public decimal Value { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Product Product { get; set; }
    }
}
