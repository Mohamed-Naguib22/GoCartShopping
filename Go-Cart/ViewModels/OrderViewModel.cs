using Go_Cart.Models;

namespace Go_Cart.ViewModels
{
    public class OrderViewModel
    {
        public IEnumerable<OrderItemViewModel> OrderItems { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalCost { get; set; }
        public string Status { get; set; }
    }
}
