using Go_Cart.Models;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Go_Cart.ViewModels
{
    public class ProductFormViewModel
    {
        [Required, StringLength(250)]
        public string Name { get; set; }
        [Required, StringLength(2500)]
        public string Description { get; set; }
        [Range(1, double.MaxValue, ErrorMessage = "Price can not be negative!")]
        public decimal Price { get; set; }
        public byte CategoryId { get; set; }
        public IFormFile? ImgFile { get; set; }
        public decimal Rating { get; set; }
        public int NumberOfItemsInStock { get; set; }
        public List<int>? SelectedSizes { get; set; }
    }
}
