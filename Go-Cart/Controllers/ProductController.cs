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
            ViewBag.Colors = new MultiSelectList(_context.Colors, "Id", "Name");
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
                ViewBag.Colors = new MultiSelectList(_context.Colors, "Id", "Name");
                return View(model);
            }

            var product = new Product
            {
                Name = model.Name,
                Price = model.Price,
                CategoryId = model.CategoryId,
                Description = model.Description,
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

            if (model.SelectedColors != null && model.SelectedColors.Any())
            {
                foreach (var selectedColorId in model.SelectedColors)
                {
                    var productColor = new ProductColor
                    {
                        ProductId = product.Id,
                        ColorId = selectedColorId
                    };

                    await _context.ProductColors.AddAsync(productColor);
                }

                await _context.SaveChangesAsync();
            }

            if (model.DiscountPercentage != null)
            {
                var offer = new Offer
                {
                    DiscountPercentage = (decimal)model.DiscountPercentage,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(30),
                    ProductId = product.Id
                };
                product.OnSale = true;
                await _context.Offers.AddAsync(offer);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), "Home");
        }
        public async Task<IActionResult> Details(int productId)
        {
            var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == productId);
            
            var reviews = _context.ProductReviews
                .Where(r => r.ProductId == productId)
                .Include(r => r.ApplicationUser)
                .OrderByDescending(r => r.AddedOn);
            
            var sizes = _context.ProductSizes
                .Where(ps => ps.ProductId == productId)
                .Select(ps => ps.Size.Name)
                .ToList();

            var colors = _context.ProductColors
                .Where(pc => pc.ProductId == productId)
                .Select(pc => pc.Color.Value)
                .ToList();

            var reviewViewModel = await reviews.Select(r => new ProductReviewViewModel
            {
                Id = r.Id,
                Review = r.Review,
                ReviewDate = r.AddedOn,
                UserName = r.ApplicationUser.FirstName + " " + r.ApplicationUser.LastName,
                ProfilePicture = r.ApplicationUser.ImgUrl
            }).ToListAsync();

            var ratings = await _context.Ratings.
                Where(r => r.ProductId == productId).ToListAsync();

            decimal rating = ratings.Count > 0 ? ratings.Average(r => r.Value) : 5;

            var productDetails = new ProductDetailsViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Rating = rating,
                Description = product.Description,
                ImgUrl = product.ImgUrl,
                Category = product.Category.Name,
                ProductReviews = reviewViewModel,
                AvailableSizes = sizes,
                AvailableColors = colors
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
            return Ok();
        }
        [HttpGet]
        public IActionResult GetComments(int productId)
        {
            var reviews = _context.ProductReviews
                .Where(r => r.ProductId == productId)
                .Include(r => r.ApplicationUser)
                .OrderByDescending(r => r.AddedOn)
                .ToList();

            var reviewViewModel = reviews.Select(r => new ProductReviewViewModel
            {
                Id = r.Id,
                Review = r.Review,
                ReviewDate = r.AddedOn,
                UserName = r.ApplicationUser.FirstName + " " + r.ApplicationUser.LastName,
                ProfilePicture = r.ApplicationUser.ImgUrl
            }).ToList();

            return PartialView("_CommentsPartial", reviewViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> AutoComplete(string searchTerm)
        {
            if (searchTerm is null)
                return Ok();

            var searchItemTrimed = searchTerm.Trim().ToLower();
            var matchingProducts = await _context.Products
                .Where(p => p.Name.ToLower().Contains(searchItemTrimed))
                .OrderBy(p => p.Name)
                .Select(p => p.Name).Distinct()
                .ToListAsync();

            return Json(matchingProducts);
        }
    }
}
