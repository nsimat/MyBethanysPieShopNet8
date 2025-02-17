using BethanysPieShopNet8.Models;
using BethanysPieShopNet8.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShopNet8.Components
{
    public class ShoppingCartSummary : ViewComponent
    {
        private readonly IShoppingCart _shoppingCart;

        public ShoppingCartSummary(IShoppingCart shoppingCart)
        {
            _shoppingCart = shoppingCart ?? throw new ArgumentNullException(nameof(shoppingCart));
        }

        public IViewComponentResult Invoke()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            var shoppingCartViewModel = new ShoppingCartViewModel(_shoppingCart,
                _shoppingCart.GetShoppingCartTotal());

            return View(shoppingCartViewModel);
        }
    }
}
