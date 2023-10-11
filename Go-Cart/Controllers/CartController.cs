using Go_Cart.Data;
using Go_Cart.Extensions;
using Go_Cart.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

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
            SetCart(cart);

            return View(cart);
        }
        public IActionResult RefreshCartPartial(string partialView)
        {
            var cart = GetCart();
            switch (partialView)
            {
                case "totalCost":
                    cart.TotalPrice = cart.Products.Sum(p => p.Price * p.Quantity);
                    SetCart(cart);
                    decimal totalCost = cart.TotalPrice;
                    return PartialView("_TotalCostPartial", totalCost);

                case "cartSlider":
                    return PartialView("_CartSliderPartial", cart.Products);

                case "cartItems":
                    return PartialView("_CartPartial", cart);
            }
            return Ok();
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
        public IActionResult SetItemColor(int productId, string color)
        {
            var cart = GetCart();
            if (cart != null)
            {
                var cartItem = cart.Products.FirstOrDefault(p => p.Id == productId);

                if (cartItem != null)
                {
                    cartItem.SelecetdColor = color;
                    SetCart(cart);
                }
            }

            return Ok();
        }
        public IActionResult SetItemSize(int productId, string size)
        {
            var cart = GetCart();
            if (cart != null)
            {
                var cartItem = cart.Products.FirstOrDefault(p => p.Id == productId);

                if (cartItem != null)
                {
                    cartItem.SelectedSize = size;
                    SetCart(cart);
                }
            }

            return Ok();
        }
        private CartItem? GetProductById(int productId)
        {
            var product = _context.Products
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Id == productId);
            
            var sizes = _context.ProductSizes
                .Where(ps => ps.ProductId == productId)
                .Select(ps => ps.Size.Name)
                .ToList();

            var colors = _context.ProductColors
                .Where(pc => pc.ProductId == productId)
                .Select(pc => pc.Color.Name)
                .ToList();

            var cartItem = new CartItem
            {
                Id = productId,
                Name = product.Name,
                Category = product.Category.Name,
                ImgUrl = product.ImgUrl,
                Price = product.Price,
                Quantity = 1,
                Sizes = sizes,
                Colors = colors
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
