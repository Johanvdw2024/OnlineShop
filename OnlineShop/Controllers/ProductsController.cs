using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Services.Interface;

namespace OnlineShop.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductsController(AppDbContext context, IProductRepoService productRepoService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await context.Products.ToListAsync());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productsModel = await context.Products.FirstOrDefaultAsync(m => m.Id == id);

            if (productsModel == null)
            {
                return NotFound();
            }

            return View(productsModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productsModel = await context.Products.FindAsync(id);

            if (productsModel == null)
            {
                return NotFound();
            }
            return View(productsModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,ProductName,Price,ItemDescription,IsAvailable,Created,Updated")] ProductsModel productsModel, IFormFile ProductImage)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingProduct = await context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
                    
                    productsModel.Created = existingProduct.Created;
                    
                    productsModel.Updated = DateTime.Now;

                    context.Update(productsModel);

                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsModelExists(productsModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(productsModel);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productsModel = await context.Products.FirstOrDefaultAsync(m => m.Id == id);

            if (productsModel == null)
            {
                return NotFound();
            }

            return View(productsModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var productsModel = await context.Products.FindAsync(id);

            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ProductsModelExists(Guid id)
        {
            return context.Products.Any(e => e.Id == id);
        }
    }
}
