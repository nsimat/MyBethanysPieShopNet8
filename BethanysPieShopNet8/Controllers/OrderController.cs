using BethanysPieShopNet8.Models;
using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShopNet8.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IShoppingCart _shoppingCart;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderRepository orderRepository,
                               IShoppingCart shoppingCart,
                               ILogger<OrderController> logger)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _shoppingCart = shoppingCart ?? throw new ArgumentNullException(nameof(shoppingCart));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IActionResult Checkout()
        {
            _logger.LogInformation("Loading the checkout form...");
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            _logger.LogInformation("Checking out order...");

            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            if (_shoppingCart.ShoppingCartItems.Count == 0)
            {
                ModelState.AddModelError("", "Your cart is empty, add some pies first.");
            }

            if (ModelState.IsValid)
            {
                _orderRepository.CreateOrder(order);
                _shoppingCart.ClearCart();
                return RedirectToAction("CheckoutComplete");
            }

            return View(order);
        }

        public IActionResult CheckoutComplete()
        {
            _logger.LogInformation("Completing the final checkout...");

            ViewBag.CheckoutCompleteMessage = "Thanks for your order. You'll soon enjoy our delicious pies!";
            return View();
        }
    }
}
