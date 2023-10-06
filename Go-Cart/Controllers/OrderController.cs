using Go_Cart.Data;
using Go_Cart.Extensions;
using Go_Cart.Models;
using Go_Cart.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Go_Cart.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> PlaceOrder()
        {
            Cart cart = HttpContext.Session.Get<Cart>("_cart");
            var totalCost = cart.Products.Sum(item => item.Price * item.Quantity);
            var user = await _userManager.GetUserAsync(User);
            var order = new Order
            {
                TotalCost = totalCost,
                Status = "Waiting",
                PlacedOn = DateTime.UtcNow.ToLocalTime(),
                ApplicationUserId = user.Id
            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            foreach (var product in cart.Products)
            {
                var orderItem = new OrderDetails
                {
                    ProductId = product.Id,
                    OrderId = order.Id,
                    Quantity = product.Quantity,
                    SubTotalPrice = product.Price * product.Quantity
                };
                await _context.OrderDetails.AddAsync(orderItem);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Checkout", new { orderId = order.Id });
        }
        public async Task<IActionResult> Checkout(int orderId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

            var orderItems = _context.OrderDetails.Include(oi => oi.Product).Where(oi => oi.OrderId == orderId);
            
            var orderSubmitFormViewModel = new OrderSubmitFormViewModel
            {
                Id = orderId,
                TotalCost = order.TotalCost,
                PlacedOn = order.PlacedOn,
                OrderItems = orderItems,
            };
            return View(orderSubmitFormViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAddress(OrderSubmitFormViewModel model)
        {
            var address = new ShippingAddress
            {
                OrderId = model.Id,
                ZipCode = model.ZipCode,
                State = model.State,
                City = model.City,
                Street = model.Street,
                RecipientName = model.RecipientName,
                RecipientPhone  = model.RecipientPhone,
            };

            await _context.ShippingAddresses.AddAsync(address);
            await _context.SaveChangesAsync();
            return RedirectToAction("ProcessPayment", new { orderId = model.Id, paymentMethod = model.PaymentMethod });
        }
        public IActionResult ProcessPayment(int orderId, string paymentMethod)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null)
            {
                return NotFound();
            }

            var paymentGateway = new PaymentGateway();
            var paymentResult = paymentGateway.ProcessPayment(order.TotalCost);

            if (paymentResult.Success)
            {
                order.IsPaid = true;
                _context.SaveChanges();

                var payment = new Transaction
                {
                    OrderId = orderId,
                    Amount = order.TotalCost,
                    PaymentDate = DateTime.UtcNow.ToLocalTime(),
                    PaymentMethod = paymentMethod,
                    BuyerId = order.ApplicationUserId
                };
                _context.Transactions.Add(payment);
                _context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            else
            {
                order.IsPaid = false;
                _context.SaveChanges();

                return RedirectToAction("PaymentFailure", new { orderId = order.Id });
            }
        }

        public IActionResult PaymentSuccess(int orderId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        public IActionResult PaymentFailure(int orderId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}
