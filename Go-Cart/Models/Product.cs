namespace Go_Cart.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public int NumberOfItemsInStock { get; set; }
        public int NumberOfSales { get; set; }
        public bool OnSale { get; set; }
        public bool IsActive { get; set; }
        public Category Category { get; set; }
        public Offer Offer { get; set; }
        public IEnumerable<ProductReview> ProductReviews { get; set; }
        public IEnumerable<ProductColor> ProductColors { get; set; }
        public IEnumerable<ProductSize> ProductSizes { get; set; }
        public IEnumerable<OrderItem> ProductOrders { get; set; }
        public IEnumerable<WishListItem> WishListItems { get; set; }
        public IEnumerable<Rating> Ratings { get; set; }
        public IEnumerable<ProductImage> ProductImages { get; set; }
    }
}
