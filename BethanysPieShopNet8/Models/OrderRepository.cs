namespace BethanysPieShopNet8.Models
{
    public class OrderRepository : IOrderRepository
    {
        private readonly BethanysPieShopDbContext _bethanysPieShopDbContext;
        private readonly ShoppingCart _shoppingCart;

        public OrderRepository(BethanysPieShopDbContext bethanysPieShopDbContext, ShoppingCart shoppingCart)
        {
            _bethanysPieShopDbContext = bethanysPieShopDbContext ?? throw new ArgumentNullException(nameof(bethanysPieShopDbContext));
            _shoppingCart = shoppingCart ?? throw new ArgumentNullException(nameof(shoppingCart));
        }
        public void CreateOrder(Order order)
        {
            order.OrderPlaced = DateTime.Now;

            List<ShoppingCartItem>? shoppingCartItems = _shoppingCart.ShoppingCartItems;
            order.OrderTotal = _shoppingCart.GetShoppingCartTotal();

            order.OrderDetails = new List<OrderDetail>();

            // Adding an order with its details

            foreach (ShoppingCartItem? shoppingCartItem in shoppingCartItems)
            {
                var orderDetail = new OrderDetail
                {
                    Amount = shoppingCartItem.Amount,
                    PieId = shoppingCartItem.Pie.PieId,
                    Price = shoppingCartItem.Pie.Price
                };

                order.OrderDetails.Add(orderDetail);
            }

            _bethanysPieShopDbContext.Orders.Add(order);

            _bethanysPieShopDbContext.SaveChanges();
        }
    }
}
