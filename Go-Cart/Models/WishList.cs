using Microsoft.EntityFrameworkCore;

namespace Go_Cart.Models
{
    public class WishList
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public IEnumerable<WishListItem> WishListItems { get; set; }
    }
}
