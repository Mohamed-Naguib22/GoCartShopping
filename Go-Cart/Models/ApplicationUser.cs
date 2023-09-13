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
        public IEnumerable<ProductReview> ProductReviews { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}
