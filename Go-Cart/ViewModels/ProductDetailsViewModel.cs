using Go_Cart.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Go_Cart.ViewModels
{
    public class ProductDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal? PriceAfterDiscount { get; set; }
        public decimal Rating { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string ImgUrl { get; set; }
        public bool OnSale { get; set; }
        public int NumberOfItemsInStock { get; set; }
        public List<string> AvailableSizes { get; set; }
        public List<string> AvailableColors { get; set; }
        public IEnumerable<string> ProductImages { get; set; }
        public IEnumerable<ProductReviewViewModel> ProductReviews { get; set; }
    }
}
