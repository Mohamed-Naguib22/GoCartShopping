﻿using Go_Cart.Models;

namespace Go_Cart.ViewModels
{
    public class OrderSubmitFormViewModel
    {
        public int Id { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime PlacedOn { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string RecipientName { get; set; }
        public string RecipientPhone { get; set; }
    }
}
