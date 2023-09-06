namespace Go_Cart.Models
{
    public class Cart
    {
        public Cart()
        {
            Products = new List<Product>();
        }
        public List<Product> Products { get; set; }
        public decimal TotalPrice { get; set; }
    }
}