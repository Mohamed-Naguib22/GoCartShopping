using Go_Cart.Data;
using Go_Cart.Extensions;
using Go_Cart.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Go_Cart.Controllers
{
    public class CartController : Controller
    {
        private const string CartSession = "_cart";
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cart = GetCart();
            cart.TotalPrice = cart.Products.Sum(p => p.Price * p.Quantity);

            return View(cart);
        }
        public IActionResult AddToCart(int productId)
        {
            var product = GetProductById(productId);
            var cart = GetCart();
            bool productAlreadyAdded = cart.Products.Any(p => p.Id == productId);
            if (productAlreadyAdded)
            {
                TempData["DuplicateProduct"] = true;
                return RedirectToAction(nameof(Index), "Home");
            }

            if (product != null)
            {
                cart.Products.Add(product);
            }

            SetCart(cart);

            return RedirectToAction(nameof(Index));
        }
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = GetCart();
            if (cart != null)
            {
                var product = cart.Products.FirstOrDefault(p => p.Id == productId);

                if (product != null)
                {
                    cart.Products.Remove(product);
                    SetCart(cart);
                }
            }

            return Ok();
        }
        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            var cart = GetCart();
            if (cart != null)
            {
                var cartItem = cart.Products.FirstOrDefault(p => p.Id == productId);

                if (cartItem != null)
                {
                    cartItem.Quantity = quantity;
                    SetCart(cart);
                }
            }

            return Ok();
        }
        private CartItem? GetProductById(int productId)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            var cartItem = new CartItem
            {
                Id = productId,
                Name = product.Name,
                ImgUrl = product.ImgUrl,
                Price = product.Price,
                Quantity = 1
            };
            return cartItem;
        }
        private Cart GetCart()
        {
            var cart = HttpContext.Session.Get<Cart>(CartSession);
            return cart ?? new Cart();
        }
        private void SetCart(Cart cart)
        {
            HttpContext.Session.Set(CartSession, cart);
        }
    }
}
