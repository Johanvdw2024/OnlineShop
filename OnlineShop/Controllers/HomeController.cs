using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Controllers
{
    public class HomeController(AppDbContext context) : Controller
    {
        private readonly AppDbContext _context = context;

        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public ActionResult ProductDetails(Guid id)
        {
            var productsModel = _context.Products.Find(id);

            if (productsModel.IsAvailable == false)
            {
                return RedirectToAction(nameof(PageNotFound));
            }
            else
            {
                return View(productsModel);
            }
        }

        public IActionResult PageNotFound()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
