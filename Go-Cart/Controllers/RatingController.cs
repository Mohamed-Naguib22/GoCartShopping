using Go_Cart.Data;
using Go_Cart.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Go_Cart.Controllers
{
    public class RatingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public RatingController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Rate(int productId, decimal value)
        {
            var user = await _userManager.GetUserAsync(User);

            var existingRating = await _context.Ratings
                .FirstOrDefaultAsync(r => r.ProductId == productId && r.ApplicationUserId == user.Id);

            if (existingRating != null)
            {
                existingRating.Value= value;
                _context.Update(existingRating);
            }

            else
            {
                var rating = new Rating
                {
                    ProductId = productId,
                    ApplicationUserId = user.Id,
                    Value = value
                };
                await _context.AddAsync(rating);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
        public async Task<IActionResult> GetRating(int productId)
        {
            var ratings = await _context.Ratings.
                Where(r => r.ProductId == productId).ToListAsync();

            decimal rating = ratings.Count > 0 ? ratings.Average(r => r.Value) : 5;

            return PartialView("_RatingPartial", rating);
        }
        public async Task<IActionResult> GetUserRating(int productId)
        {
            var user = await _userManager.GetUserAsync(User);

            var existingRating = await _context.Ratings
                .FirstOrDefaultAsync(r => r.ProductId == productId && r.ApplicationUserId == user.Id);
            
            if (existingRating == null)
            {
                return Json(0.0m);
            }
            return Json(existingRating.Value);
        }
    }
}
