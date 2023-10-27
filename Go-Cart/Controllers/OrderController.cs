using Go_Cart.Data;
using Go_Cart.Extensions;
using Go_Cart.Models;
using Go_Cart.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;

namespace Go_Cart.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private const string OrderSession = "_order";

        public OrderController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        private Order GetOrderSession()
        {
            var order = HttpContext.Session.Get<Order>(OrderSession);
            return order ?? new Order();
        }
        private void SetOrderSession(Order order)
        {
            HttpContext.Session.Set(OrderSession, order);
        }
        public async Task<IActionResult> PlaceOrder()
        {
            Cart cart = HttpContext.Session.Get<Cart>("_cart");
            var totalCost = cart.TotalPrice;
            var user = await _userManager.GetUserAsync(User);
            var order = new Order
            {
                TotalCost = totalCost,
                Status = "Shipped",
                PlacedOn = DateTime.Now,
                ApplicationUserId = user.Id,
                ProductOrders = new List<OrderItem>()
            };
            SetOrderSession(order);

            foreach (var cartItem in cart.Products)
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == cartItem.Id);

                if (cartItem.Quantity >= product.NumberOfItemsInStock)
                {
                    TempData["ErrorMessage"] = $"{product.Name} is out of stock.";
                    return RedirectToAction("Index", "Cart");
                }

                var orderItem = new OrderItem
                {
                    ProductId = cartItem.Id,
                    Quantity = cartItem.Quantity,
                    Color = cartItem.SelecetdColor,
                    Size = cartItem.SelectedSize,
                    SubTotalPrice = cartItem.Price * cartItem.Quantity
                };
                order.ProductOrders.Add(orderItem);
                SetOrderSession(order);

            }

            return RedirectToAction("AddAddress");
        }
        [HttpGet]
        public async Task<IActionResult> AddAddress()
        {
            var order = GetOrderSession();
            var orderItems = order.ProductOrders.ToList();
            //var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

            //var orderItems = await _context.OrderItems
            //    .Include(oi => oi.Product)
            //    .ThenInclude(p => p.ProductImages)
            //    .Where(oi => oi.OrderId == orderId).ToListAsync();

            var orderSubmitFormViewModel = new OrderSubmitFormViewModel
            {
                //Id = orderId,
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
            var orderSession = GetOrderSession();

            var order = new Order
            {
                TotalCost = orderSession.TotalCost,
                Status = orderSession.Status,
                PlacedOn = orderSession.PlacedOn,
                ApplicationUserId = orderSession.ApplicationUserId,
            };
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            foreach (var orderItemSession in orderSession.ProductOrders)
            {
                var orderItem = new OrderItem
                {
                    ProductId = orderItemSession.ProductId,
                    OrderId = order.Id,
                    Size = orderItemSession.Size,
                    Color = orderItemSession.Color,
                    SubTotalPrice = orderItemSession.SubTotalPrice,
                    Quantity = orderItemSession.Quantity,
                };
                await _context.OrderItems.AddAsync(orderItem);
            }

            var address = new ShippingAddress
            {
                OrderId = order.Id,
                ZipCode = model.ZipCode,
                City = model.City,
                Street = model.Street,
                RecipientName = model.RecipientName,
                RecipientPhone = model.RecipientPhone,
            };
            HttpContext.Session.Remove("_order");
            await _context.ShippingAddresses.AddAsync(address);
            await _context.SaveChangesAsync();
            return RedirectToAction("Checkout", new { orderId = order.Id });
        }
        public async Task<IActionResult> Checkout(int orderId)
        {
            Cart cart = HttpContext.Session.Get<Cart>("_cart");
            var user = await _userManager.GetUserAsync(User);
            var domain = "https://localhost:7285/";
            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"Order/OrderConfirmation?orderId={orderId}",
                CancelUrl = domain,
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
            var order = _context.Orders
                .Include(o => o.ProductOrders)
                .ThenInclude(o => o.Product)
                .FirstOrDefault(o => o.Id == orderId);

            foreach (var orderItem in order.ProductOrders)
            {
                orderItem.Product.NumberOfItemsInStock -= orderItem.Quantity;
                orderItem.Product.NumberOfSales += orderItem.Quantity;
            }

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
                return View("Index", "Home");
            }
        }
        public IActionResult Success()
        {
            return View();
        }
        public async Task<IActionResult> OrderHistory()
        {
            var user = await _userManager.GetUserAsync(User);
            var orders = await _context.Orders
                .Where(o => o.ApplicationUserId == user.Id)
                .Where(o => o.IsPaid)
                .Select(o => new OrderViewModel
                {
                    OrderDate = o.PlacedOn,
                    Status = o.Status,
                    TotalCost = o.TotalCost,
                    OrderItems = o.ProductOrders.Select(oi => new OrderItemViewModel
                    {
                        ProductName = oi.Product.Name,
                        ProductImg = oi.Product.ProductImages.Select(pi => pi.ImgUrl).FirstOrDefault(),
                        SubTotalPrice = oi.SubTotalPrice,
                        Color = oi.Color,
                        Size = oi.Size,
                        Quantity = oi.Quantity
                    })
                })
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }
    }
}
