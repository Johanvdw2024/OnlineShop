using OnlineShop.Models;

namespace OnlineShop.Services.Interface
{
    public interface IProductRepoService
    {
        public bool Add(ProductsModel product);
    }
}
