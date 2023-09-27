namespace Go_Cart.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImgUrl { get; set; }
    }
}
