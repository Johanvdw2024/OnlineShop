namespace OnlineShop.ViewModels
{
    public class ShoppingCartRemoveViewModel
    {
        public required string Message { get; set; }

        public int CartTotal { get; set; }

        public int ItemCount { get; set; }

        public int DeleteId { get; set; }
    }
}
