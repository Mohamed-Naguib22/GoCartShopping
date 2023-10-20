namespace Go_Cart.Models
{
    public class SearchViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public string? SearchItem { get; set; }
    }
}
