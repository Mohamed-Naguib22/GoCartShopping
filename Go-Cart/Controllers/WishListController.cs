using Go_Cart.Data;
using Go_Cart.Models;
using Go_Cart.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Go_Cart.Controllers
{
    public class WishListController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public WishListController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetWishListItems()
        {
            var user = await _userManager.GetUserAsync(User);
            var wishList = await _context.WishLists.FirstOrDefaultAsync(w => w.ApplicationUserId == user.Id);
            var whishListItems = await _context.WishListItems
                .Where(wi => wi.WishListId == wishList.Id)
                .Include(wi => wi.Product)
                .ToListAsync();

            return PartialView("_WishListPartial", whishListItems);
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var wishList = await _context.WishLists.FirstOrDefaultAsync(w => w.ApplicationUserId == user.Id);
            var whishListItems = await _context.WishListItems
                .Where(wi => wi.WishListId == wishList.Id)
                .Include(wi => wi.Product)
                .Include(p => p.Product.ProductImages)
                .Include(p => p.Product.Ratings)
                .ToListAsync();

            return View(whishListItems);
        }
        public async Task<IActionResult> AddToWishList(int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            var wishList = await _context.WishLists.FirstOrDefaultAsync(w => w.ApplicationUserId == user.Id);

            var wishListItem = new WishListItem
            {
                ProductId = productId,
                WishListId = wishList.Id,
                AddedOn = DateTime.Now
            };
            await _context.WishListItems.AddAsync(wishListItem);
            await _context.SaveChangesAsync();

            return Ok();
        }
        public async Task<IActionResult> RemoveFromWishList(int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            var wishList = await _context.WishLists.FirstOrDefaultAsync(w => w.ApplicationUserId == user.Id);

            var wishListItem = await _context.WishListItems
                .SingleOrDefaultAsync(wi => wi.WishListId == wishList.Id && wi.ProductId == productId);

            _context.WishListItems.Remove(wishListItem);
            await _context.SaveChangesAsync();

            return Ok();
        }
        public async Task<IActionResult> IsAddedToWishlist(int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            var wishList = await _context.WishLists.FirstOrDefaultAsync(w => w.ApplicationUserId == user.Id);

            var isAdded = _context.WishListItems
                .Any(wi => wi.WishListId == wishList.Id && wi.ProductId == productId);

            return Ok(isAdded);
        }
    }
}