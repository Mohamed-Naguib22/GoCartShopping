using Go_Cart.Data;
using Go_Cart.Interfaces;
using Go_Cart.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Search(string query)
        {
            var productNameTrimed = query.Trim().ToLower();
            var matchingProducts = await _context.Products
                .Where(p => p.Name.ToLower().Contains(productNameTrimed))
                .OrderBy(p => p.Name).ToListAsync();
            
            return View(matchingProducts);
        }
    }
}
