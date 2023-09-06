using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Go_Cart.Models
{
    public class ProductReview
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ApplicationUserId { get; set; }
        public Product Product { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string Review { get; set; }
    }
}
