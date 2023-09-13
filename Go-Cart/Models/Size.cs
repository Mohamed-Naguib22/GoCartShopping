namespace Go_Cart.Models
{
    public class Size
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<ProductSize> ProductSizes { get; set;}
    }
}
