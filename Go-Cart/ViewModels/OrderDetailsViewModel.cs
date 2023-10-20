using Go_Cart.Models;
using System.ComponentModel.DataAnnotations;

namespace Go_Cart.ViewModels
{
    public class OrderSubmitFormViewModel
    {
        public int Id { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime PlacedOn { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string ZipCode { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string RecipientName { get; set; }
        [Required]
        public string RecipientPhone { get; set; }
    }
}
