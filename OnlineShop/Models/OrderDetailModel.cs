using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models
{
    public class OrderDetailModel
    {
        [Key]

        public Guid ProductId { get; set; }

        public int OrderId { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }

        public virtual required OrderModel OrderModel { get; set; }

        public virtual required ProductsModel ProductsModels { get; set; }
    }
}
