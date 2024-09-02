using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Services.Implementation;
using OnlineShop.ViewModels;

namespace OnlineShop.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartController(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public ActionResult Index()
        {
            var cart = CartActions.GetCart(_context, _httpContextAccessor);

            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };

            ViewData["CartTotal"] = cart.GetCartItems().Count;

            return View(viewModel);
        }
        public IActionResult AddToCart(Guid productId)
        {
            var addedProduct = _context.Products.Single(
                product => product.Id == productId
                );

            var cart = CartActions.GetCart(_context, _httpContextAccessor);

            cart.AddToCart(addedProduct);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveFromCart(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartModel = await _context.Carts.FindAsync(id);

            if (cartModel != null)
            {
                _context.Carts.Remove(cartModel);
            }
            await _context.SaveChangesAsync();

            return RedirectToActionPermanent(nameof(Index));
        }

        public ActionResult CartSummary()
        {
            var cart = CartActions.GetCart(_context, _httpContextAccessor);

            ViewData["CartTotal"] = cart.GetCount();

            return PartialView("CartSummary");
        }
    }
}
