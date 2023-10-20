using Go_Cart.Data;
using Go_Cart.Interfaces;
using Go_Cart.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System.Linq;

namespace Go_Cart.Controllers
{
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext _context;
        
        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Search(string searchItem)
        {
            if (string.IsNullOrEmpty(searchItem))
            {
                return Ok();
            }
            var productNameTrimed = searchItem.Trim().ToLower();

            var query = _context.Products
                .Include(p => p.Ratings)
                .Include(p => p.Offer)
                .Include(p => p.ProductImages);

            var matchingProducts = await query
                .Where(p => p.Name.ToLower().Contains(productNameTrimed))
                .OrderBy(p => p.Name).ToListAsync();


            if (!matchingProducts.Any())
            {
                var products = await query.ToListAsync();
            }

            var searhViewModel = new SearchViewModel
            {
                Products = matchingProducts,
                SearchItem = searchItem
            };

            return View(searhViewModel);
        }
    }
}
