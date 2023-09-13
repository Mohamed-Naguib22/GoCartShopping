using Go_Cart.Data;
using Go_Cart.Interfaces;
using Go_Cart.Models;
using Go_Cart.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Go_Cart.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageService<Product> _productImage;
        private readonly IDateService<Product> _productDateService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductController(ApplicationDbContext context,
            IImageService<Product> productImage,
            IDateService<Product> productDateService,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _productImage = productImage;
            _productDateService = productDateService;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync();
            return View(products);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            ViewBag.Sizes = new MultiSelectList(_context.Sizes, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", model.CategoryId);
                ViewBag.Sizes = new MultiSelectList(_context.Sizes, "Id", "Name");
                return View(model);
            }

            var product = new Product
            {
                Name = model.Name,
                Price = model.Price,
                CategoryId = model.CategoryId,
                Description = model.Description,
                Rating = model.Rating,
                NumberOfItemsInStock = model.NumberOfItemsInStock,
                NumberOfSales = 0,
                IsActive = true,
            };

            _productImage.SaveImage(model.ImgFile, product);
            _productDateService.SetDatesToNow(product);

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            if (model.SelectedSizes != null && model.SelectedSizes.Any())
            {
                foreach (var selectedSizeId in model.SelectedSizes)
                {
                    var productSize = new ProductSize
                    {
                        ProductId = product.Id,
                        SizeId = selectedSizeId
                    };

                    await _context.ProductSizes.AddAsync(productSize);
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), "Home");
        }
        public async Task<IActionResult> Details(int productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
            
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == product.CategoryId);
            
            var reviews = _context.ProductReviews
                .Where(r => r.ProductId == productId)
                .Include(r => r.ApplicationUser);
            
            var Sizes = _context.ProductSizes
                .Where(ps => ps.ProductId == productId)
                .Include(ps => ps.Size);

            var reviewViewModel = reviews.Select(r => new ProductReviewViewModel
            {
                Id = r.Id,
                Review = r.Review,
                UserName = r.ApplicationUser.FirstName + " " + r.ApplicationUser.LastName,
                ProfilePicture = r.ApplicationUser.ImgUrl
            });

            var availableSizes = new List<string>();

            foreach (var size in Sizes)
            {
                availableSizes.Add(size.Size.Name);
            }
            
            var productDetails = new ProductDetailsViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Rating = product.Rating,
                Description = product.Description,
                ImgUrl = product.ImgUrl,
                Category = category.Name,
                ProductReviews = reviewViewModel,
                AvailableSizes = availableSizes
            };

            return View(productDetails);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview(string review, int productId)
        {
            var user = await _userManager.GetUserAsync(User);

            var productReview = new ProductReview
            {
                Review = review,
                AddedOn = DateTime.Now,
                ProductId = productId,
                ApplicationUserId = user.Id
            };
            if (review is not null)
            {
                await _context.ProductReviews.AddAsync(productReview);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index), "Home");
        }
    }
}
