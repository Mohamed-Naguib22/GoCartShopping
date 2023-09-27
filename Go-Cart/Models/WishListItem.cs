namespace Go_Cart.Models
{
    public class WishListItem
    {
        public int WishListId { get; set; }
        public int ProductId { get; set; }
        public DateTime AddedOn { get; set; }
        public WishList WishList { get; set; }
        public Product Product { get; set; }
    }
}
