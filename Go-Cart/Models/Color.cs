namespace Go_Cart.Models
{
    public class Color
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public IEnumerable<ProductColor> ProductColors { get; set; }
    }
}