using Go_Cart.Data;
using Go_Cart.Extensions;
using Go_Cart.Models;
using Go_Cart.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using Stripe.Checkout;
using Stripe;

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
            var totalCost = cart.TotalPrice;
            var user = await _userManager.GetUserAsync(User);
            var order = new Order
            {
                TotalCost = totalCost,
                Status = "Waiting",
                PlacedOn = DateTime.Now,
                ApplicationUserId = user.Id
            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            foreach (var product in cart.Products)
            {
                var orderItem = new OrderItem
                {
                    ProductId = product.Id,
                    OrderId = order.Id,
                    Quantity = product.Quantity,
                    Color = product.SelecetdColor,
                    Size = product.SelectedSize,
                    SubTotalPrice = product.Price * product.Quantity
                };
                await _context.OrderItems.AddAsync(orderItem);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("AddAddress", new { orderId = order.Id });
        }
        [HttpGet]
        public async Task<IActionResult> AddAddress(int orderId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

            var orderItems = _context.OrderItems.Include(oi => oi.Product).Where(oi => oi.OrderId == orderId);

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
                City = model.City,
                Street = model.Street,
                RecipientName = model.RecipientName,
                RecipientPhone = model.RecipientPhone,
            };

            await _context.ShippingAddresses.AddAsync(address);
            await _context.SaveChangesAsync();
            return RedirectToAction("Checkout", new { orderId = model.Id });
        }
        public async Task<IActionResult> Checkout(int orderId)
        {
            Cart cart = HttpContext.Session.Get<Cart>("_cart");
            var user = await _userManager.GetUserAsync(User);
            var domain = "https://localhost:7285/";
            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"Order/OrderConfirmation?orderId={orderId}",
                CancelUrl = domain + $"Order/Failed",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                CustomerEmail = user.Email,
            };

            foreach (var item in cart.Products)
            {
                var sessionListItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100),
                        Currency = "egp",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Name,
                        }
                    },
                    Quantity = item.Quantity,
                };
                options.LineItems.Add(sessionListItem);
            }
            var service = new SessionService();
            Session session = service.Create(options);

            TempData["Session"] = session.Id;

            Response.Headers.Add("Location", session.Url);

            return new StatusCodeResult(303);
        }
        public async Task<IActionResult> OrderConfirmation(int orderId)
        {
            var service = new SessionService();
            Session session = service.Get(TempData["Session"].ToString());
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);

            if (session.PaymentStatus == "paid")
            {
                order.IsPaid = true;

                var transactionId = session.PaymentIntentId.ToString();
                var paymentMethod = session.PaymentMethodTypes.FirstOrDefault();

                var transaction = new Transaction
                {
                    OrderId = orderId,
                    TransactionId = transactionId,
                    Amount = order.TotalCost,
                    PaymentDate = DateTime.UtcNow.ToLocalTime(),
                    PaymentMethod = paymentMethod,
                    BuyerId = order.ApplicationUserId
                };
                await _context.Transactions.AddAsync(transaction);
                await _context.SaveChangesAsync();
                HttpContext.Session.Remove("_cart");
                return View("Success");
            }
            else
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                return View("Failed");
            }
        }
        public IActionResult Success()
        {
            return View();
        }
        public IActionResult Failed()
        {
            return View();
        }


        //public IActionResult ProcessPayment(int orderId, string paymentMethod)
        //{
        //    var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    var paymentGateway = new PaymentGateway();
        //    var paymentResult = paymentGateway.ProcessPayment(order.TotalCost);

        //    if (paymentResult.Success)
        //    {
        //        order.IsPaid = true;
        //        _context.SaveChanges();

        //        var payment = new Transaction
        //        {
        //            OrderId = orderId,
        //            Amount = order.TotalCost,
        //            PaymentDate = DateTime.UtcNow.ToLocalTime(),
        //            PaymentMethod = paymentMethod,
        //            BuyerId = order.ApplicationUserId
        //        };
        //        _context.Transactions.Add(payment);
        //        _context.SaveChanges();

        //        return RedirectToAction("Index", "Home");
        //    }
        //    else
        //    {
        //        order.IsPaid = false;
        //        _context.SaveChanges();

        //        return RedirectToAction("PaymentFailure", new { orderId = order.Id });
        //    }
        //}

        //public IActionResult PaymentSuccess(int orderId)
        //{
        //    var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(order);
        //}

        //public IActionResult PaymentFailure(int orderId)
        //{
        //    var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(order);
        //}
    }
}
