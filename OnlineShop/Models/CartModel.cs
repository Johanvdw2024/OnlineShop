using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models
{
    public class CartModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int RecordId { get; set; }

        public Guid CartId { get; set; }

        public Guid ProductId { get; set; }

        public int Count { get; set; }

        public virtual required ProductsModel Products { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }
    }
}
