using Go_Cart.Interfaces;
using Go_Cart.Models;

namespace Go_Cart.Services
{
    public class ProductDateService : IDateService<Product>
    {
        public void SetDatesToNow(Product product)
        {
            product.CreatedAt = DateTime.Now;
            product.LastUpdatedAt = DateTime.Now;
        }
        public void SetUpdateDateToNow(Product product)
        {
            product.LastUpdatedAt = DateTime.Now;
        }
    }
}