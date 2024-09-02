using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Services.Interface;

namespace OnlineShop.Services.Implementation
{
    public class CartActions : ICartActions
    {
        private readonly AppDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Guid _shoppingCartId;
        private const string CartSessionKey = "CartId";

        public CartActions(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _shoppingCartId = Guid.Parse(GetCartId());
        }

        public async Task AddToCartAsync(ProductsModel product)
        {
            var cartItem = await _dbContext.Carts
                .SingleOrDefaultAsync(c => c.CartId == _shoppingCartId && c.ProductId == product.Id);

            if (cartItem == null)
            {
                cartItem = new CartModel
                {
                    ProductId = product.Id,
                    CartId = _shoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now,
                    Products = product
                };

                await _dbContext.Carts.AddAsync(cartItem);
            }
            else
            {
                cartItem.Count++;
            }

            await _dbContext.SaveChangesAsync();
        }

        private string GetCartId()
        {
            var context = _httpContextAccessor.HttpContext;
            var session = context.Session;
            var cartId = session.GetString(CartSessionKey);

            if (string.IsNullOrEmpty(cartId))
            {
                cartId = Guid.NewGuid().ToString();
                session.SetString(CartSessionKey, cartId);
            }

            return cartId;
        }

        public async Task<int> GetCountAsync()
        {
            var count = await _dbContext.Carts
                .Where(cartItems => cartItems.CartId == _shoppingCartId)
                .SumAsync(cartItems => (int?)cartItems.Count);

            return count ?? 0;
        }

        public async Task<List<CartModel>> GetCartItemsAsync()
        {
            return await _dbContext.Carts
                .Include(cart => cart.Products)
                .Where(cart => cart.CartId == _shoppingCartId)
                .ToListAsync();
        }

        public async Task<int> GetTotalAsync()
        {
            var total = await _dbContext.Carts
                .Where(cartItems => cartItems.CartId == _shoppingCartId)
                .SumAsync(cartItems => (int?)cartItems.Count * cartItems.Products.Price);

            return total ?? 0;
        }

        public async Task<int> CreateOrderAsync(OrderModel order)
        {
            int orderTotal = 0;
            var cartItems = await GetCartItemsAsync();

            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetailModel
                {
                    ProductId = item.ProductId,
                    OrderId = order.OrderId,
                    Price = item.Products.Price,
                    Quantity = item.Count,
                    OrderModel = order,
                    ProductsModels = item.Products
                };

                orderTotal += (item.Count * item.Products.Price) / 100;

                await _dbContext.OrderDetails.AddAsync(orderDetail);
            }

            order.Total = orderTotal;
            await _dbContext.SaveChangesAsync();
            await EmptyCartAsync();

            return order.OrderId;
        }

        public async Task<int> RemoveFromCartAsync(int id)
        {
            var cartItem = await _dbContext.Carts.SingleOrDefaultAsync(cart => cart.CartId == _shoppingCartId && cart.RecordId == id);

            if (cartItem == null)
            {
                return 0;
            }

            if (cartItem.Count > 1)
            {
                cartItem.Count--;
            }
            else
            {
                _dbContext.Carts.Remove(cartItem);
            }

            await _dbContext.SaveChangesAsync();

            return cartItem.Count;
        }

        public async Task EmptyCartAsync()
        {
            var cartItems = _dbContext.Carts.Where(cart => cart.CartId == _shoppingCartId);

            _dbContext.Carts.RemoveRange(cartItems);

            await _dbContext.SaveChangesAsync();
        }

        internal static object GetCart(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            throw new NotImplementedException();
        }
    }
}
