using Go_Cart.Models;

namespace Go_Cart.ViewModels
{
    public class CategoryViewModel
    {
        public Category Category { get; set; }
        public IEnumerable<Size> Sizes { get; set; }
        public IEnumerable<Color> Colors { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
