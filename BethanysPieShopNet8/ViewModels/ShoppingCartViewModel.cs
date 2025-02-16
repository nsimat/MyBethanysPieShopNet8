using BethanysPieShopNet8.Models;

namespace BethanysPieShopNet8.ViewModels
{
    public class ShoppingCartViewModel
    {
        public ShoppingCartViewModel(IShoppingCart shoppingCart, decimal shoppingCartTotal)
        {
            ShoppingCart = shoppingCart ?? throw new ArgumentNullException(nameof(shoppingCart));
            ShoppingCartTotal = shoppingCartTotal;
        }

        public IShoppingCart ShoppingCart { get; }
        public decimal ShoppingCartTotal { get; set; }
    }
}
