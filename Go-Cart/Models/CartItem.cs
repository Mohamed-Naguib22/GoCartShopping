namespace Go_Cart.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImgUrl { get; set; }
        public DateTime AddedOn { get; set; }
        public IEnumerable<string> Sizes { get; set; }
        public IEnumerable<string> Colors { get; set; }
        public string? SelectedSize { get; set; }
        public string? SelecetdColor { get; set; }
    }
}
