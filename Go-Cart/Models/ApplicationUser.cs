using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Go_Cart.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required, MaxLength(100)]
        public string FirstName { get; set; }
        [Required, MaxLength(100)]
        public string LastName { get; set; }
        [Required]
        public string ImgUrl { get; set; }
        public string Gender { get; set; }
        public string? Address { get; set; }
        public WishList WishList { get; set; }
        public IEnumerable<ProductReview> ProductReviews { get; set; }
        public IEnumerable<Rating> Ratings { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}
