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
        //public async Task<IActionResult> Index(int categoryId)
        //{
        //    var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
        //    var sizes = await _context.Sizes.ToListAsync();
        //    var colors = await _context.Colors.ToListAsync();
        //    var query = _context.Products.Include(p => p.ProductImages).Include(p => p.Ratings);
        //    IEnumerable<Product> products;
        //    int totalItems;
        //    if (categoryId == 1)
        //    {
        //        products = await query.Where(p => p.Category.ParentCategoryId == 1).ToListAsync();
        //        totalItems = _context.Products.Where(p => p.Category.ParentCategoryId == 1).Count();
        //    }
        //    else
        //    {
        //        products = await query.Where(p => p.CategoryId == categoryId).ToListAsync();
        //        totalItems = _context.Products.Where(p => p.Category.Id == categoryId).Count();
        //    }

        //    var categoryViewModel = new CategoryViewModel
        //    {
        //        Category = category,
        //        Products = products,
        //        Sizes = sizes,
        //        Colors = colors,
        //    };

        //    return View(categoryViewModel);
        //}
        public async Task<IActionResult> Index(int categoryId, int page = 1, int pageSize = 10)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
            var sizes = await _context.Sizes.ToListAsync();
            var colors = await _context.Colors.ToListAsync();
            var query = _context.Products.Include(p => p.ProductImages).Include(p => p.Ratings);
            IEnumerable<Product> products;
            int totalItems;
            if (categoryId == 1)
            {
                products = await query.Where(p => p.Category.ParentCategoryId == 1).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
                totalItems = _context.Products.Where(p => p.Category.ParentCategoryId == 1).Count();
            }
            else
            {
                products = await query.Where(p => p.CategoryId == categoryId).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
                totalItems = _context.Products.Where(p => p.Category.Id == categoryId).Count();
            }
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var categoryViewModel = new CategoryViewModel
            {
                Category = category,
                Products = products,
                Sizes = sizes,
                Colors = colors,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = totalPages
            };

            return View(categoryViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> NewArrivals(int page = 1, int pageSize = 7)
        {
            var products = await _context.Products
                .Skip((page - 1) * pageSize).Take(pageSize)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            var sizes = await _context.Sizes.ToListAsync();
            var colors = await _context.Colors.ToListAsync();

            var totalItems = _context.Products.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var categoryViewModel = new CategoryViewModel
            {
                Products = products,
                Sizes = sizes,
                Colors = colors,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = totalPages
            };
            return View(categoryViewModel);
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
