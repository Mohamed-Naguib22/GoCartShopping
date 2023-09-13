namespace Go_Cart.Models
{
    public class ShippingAddress
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string RecipientName { get; set; }
        public string RecipientPhone { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
