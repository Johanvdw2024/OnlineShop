using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Services.Implementation;

namespace OnlineShop.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CheckoutController(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public ActionResult Payment()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Payment([FromForm][Bind("FirstName,LastName,Address,City,PostalCode,Country,UpdateAt,Email")] OrderModel order)
        {
            if (!ModelState.IsValid)
            {
                return View(order);
            }

            order.Email = User.Identity.Name;
            order.OrderDate = DateTime.Now;
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();

            var cart = CartActions.GetCart(_dbContext, _httpContextAccessor);

            return;
        }
    }
}
