namespace Go_Cart.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImgUrl { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}