using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models
{
    public class ProductsModel
    {
        [Key]

        public Guid Id { get; set; } = Guid.NewGuid();

        public string? ProductName { get; set; }

        public int Price { get; set; }

        public string? ItemDescription { get; set; }

        public bool IsAvailable { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Created { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        public DateTime? Updated { get; set; }
    }
}
