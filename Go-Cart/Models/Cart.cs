namespace Go_Cart.Models
{
    public class Cart
    {
        public Cart()
        {
            Products = new List<CartItem>();
        }
        public List<CartItem> Products { get; set; }
        public decimal TotalPrice { get; set; }
    }
}