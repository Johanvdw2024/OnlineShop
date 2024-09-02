using OnlineShop.Data;
using OnlineShop.Services.Interface;
using OnlineShop.Models;

namespace OnlineShop.Services.Implementation
{
    public class ProductRepoService(AppDbContext appDbContext) : IProductRepoService
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        public bool Add(ProductsModel product)
        {
            try
            {
                _appDbContext.Products.Add(product);
                _appDbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
