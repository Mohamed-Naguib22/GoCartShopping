using Go_Cart.Data;
using Go_Cart.Models;
using Go_Cart.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;
using System.Web;

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
            var sizes = await _context.Sizes.ToListAsync();
            var colors = await _context.Colors.ToListAsync();
            var query = _context.Products.Include(p => p.Ratings);
            var products = await query.Where(p => p.CategoryId == categoryId).ToListAsync();

            if (categoryId == 1)
            {
                products = await query.Where(p => p.Category.ParentCategoryId == 1).ToListAsync();
            }

            var categoryViewModel = new CategoryViewModel
            {
                Category = category,
                Products = products,
                Sizes = sizes,
                Colors = colors
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
        public async Task<IActionResult> FilteredProducts(int categoryId, decimal price, int? sizeId, int? colorId, string sortBy)
        {
            var query = _context.Products
                .Include(p => p.Ratings)
                .Include(p => p.Category)
                .Include(p => p.ProductSizes)
                .Include(p => p.ProductColors)
                .Where(p => p.Price <= price);

            if (categoryId != 1)
            {
                query = query.Where(p => p.CategoryId == categoryId);
            }
            else
            {
                query = query.Where(p => p.Category.ParentCategoryId == 1);
            }

            if (sizeId.HasValue)
            {
                query = query.Where(p => p.ProductSizes.Any(s => s.SizeId == sizeId));
            }
            if (colorId.HasValue)
            {
                query = query.Where(p => p.ProductColors.Any(c => c.ColorId == colorId));
            }
            if (colorId.HasValue)
            {
                query = query.Where(p => p.ProductColors.Any(c => c.ColorId == colorId));
            }

            switch (sortBy)
            {
                case "highest":
                    query = query.OrderByDescending(p => p.Price);
                    break;                
                case "lowest":
                    query = query.OrderBy(p => p.Price);
                    break;
                case "newest":
                    query = query.OrderByDescending(p => p.CreatedAt);
                    break;
            }

            var filteredProducts = await query.ToListAsync();
            return PartialView("_ProductsPartial", filteredProducts);
        }
    }
}
