
using Microsoft.EntityFrameworkCore;

namespace BethanysPieShopNet8.Models
{
    public class ShoppingCart : IShoppingCart
    {
        // Private field for the database context
        private readonly BethanysPieShopDbContext _bethanysPieShopDbContext;
        public string? ShoppingCartId { get; set; }
        public List<ShoppingCartItem> ShoppingCartItems { get; set; } = default!;

        // Private constructor to enforce the use of GetCart method
        private ShoppingCart(BethanysPieShopDbContext bethanysPieShopDbContext)
        {
            _bethanysPieShopDbContext = bethanysPieShopDbContext ??
                throw new ArgumentNullException(nameof(bethanysPieShopDbContext));
        }

        // Static method to get the shopping cart instance
        public static ShoppingCart GetCart(IServiceProvider services)
        {
            ISession? session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.Session;
            BethanysPieShopDbContext context = services.GetService<BethanysPieShopDbContext>()
                 ?? throw new Exception("Error initializing!");

            string cartId = session?.GetString("CartId") ?? Guid.NewGuid().ToString();

            session?.SetString("CartId", cartId);

            return new ShoppingCart(context) { ShoppingCartId = cartId };
        }

        // Method to add a pie to the shopping cart
        public void AddToCart(Pie pie)
        {
            var shoppingCartItem = _bethanysPieShopDbContext.ShoppingCartItems
                .SingleOrDefault(s => s.Pie.PieId == pie.PieId && s.ShoppingCartId == ShoppingCartId);

            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = ShoppingCartId,
                    Pie = pie,
                    Amount = 1
                };

                _bethanysPieShopDbContext.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Amount++;
            }
            _bethanysPieShopDbContext.SaveChanges();
        }

        // Method to clear the shopping cart
        public void ClearCart()
        {
            var cartItems = _bethanysPieShopDbContext.ShoppingCartItems
                .Where(cart => cart.ShoppingCartId == ShoppingCartId);

            _bethanysPieShopDbContext.ShoppingCartItems.RemoveRange(cartItems);

            _bethanysPieShopDbContext.SaveChanges();
        }

        // Method to get the items in the shopping cart
        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            return ShoppingCartItems ??= _bethanysPieShopDbContext.ShoppingCartItems
                .Where(sc => sc.ShoppingCartId == ShoppingCartId)
                .Include(sc => sc.Pie)
                .ToList();
        }

        // Method to get the total cost of the shopping cart
        public decimal GetShoppingCartTotal()
        {
            var total = _bethanysPieShopDbContext.ShoppingCartItems
                .Where(sc => sc.ShoppingCartId == ShoppingCartId)
                .Select(sc => sc.Pie.Price * sc.Amount).Sum();

            return total;
        }

        // Method to remove a pie from the shopping cart
        public int RemoveFromCart(Pie pie)
        {
            var shoppingCartItem = _bethanysPieShopDbContext.ShoppingCartItems
                .SingleOrDefault(s => s.Pie.PieId == pie.PieId && s.ShoppingCartId == ShoppingCartId);

            var localAmount = 0;

            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Amount > 1)
                {
                    shoppingCartItem.Amount--;
                    localAmount = shoppingCartItem.Amount;
                }
                else
                {
                    _bethanysPieShopDbContext.ShoppingCartItems.Remove(shoppingCartItem);
                }
            }

            _bethanysPieShopDbContext.SaveChanges();

            return localAmount;
        }
    }
}
