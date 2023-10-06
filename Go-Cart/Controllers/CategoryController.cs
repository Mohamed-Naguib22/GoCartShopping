using Go_Cart.Data;
using Go_Cart.Models;
using Go_Cart.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Go_Cart.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int categoryId)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
            var products = _context.Products.Where(p => p.CategoryId == categoryId);

            if (categoryId == 1)
            {
                products = _context.Products.Where(p => p.Category.ParentCategoryId == 1);
            }

            var categoryViewModel = new CategoryViewModel
            {
                Category = category,
                Products = products
            };

            return View(categoryViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> NewArrivals()
        {
            var products = await _context.Products.Take(5).ToListAsync();
            return View(products);
        }
        [HttpGet]
        public async Task<IActionResult> FilteredProducts(int categoryId, decimal price)
        {
            var products = await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();

            if (categoryId == 1)
            {
                products = await _context.Products
                    .Where(p => p.Category.ParentCategoryId == categoryId)
                    .ToListAsync();
            }

            var filteredProducts = products.Where(p => p.Price <= price);
            return PartialView("_ProductsPartial", filteredProducts);
        }
    }
}
