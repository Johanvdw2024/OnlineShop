using OnlineShop.Models;

namespace OnlineShop.ViewModels
{
    public class ShoppingCartViewModel
    {
        public required List<CartModel> CartItems { get; set; }

        public int CartTotal { get; set; }
    }
}
