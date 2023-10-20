using System.ComponentModel.DataAnnotations;

namespace Go_Cart.Models
{
    public class ShippingAddress
    {
        [Required]
        public int Id { get; set; }
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
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
