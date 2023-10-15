using Go_Cart.Models;

namespace Go_Cart.ViewModels
{
    public class OrderItemViewModel
    {
        public string ProductName { get; set; }
        public string ProductImg { get; set; }
        public decimal SubTotalPrice { get; set; }
        public int Quantity { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
    }
}
