using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Models;

namespace OnlineShop.Data
{
    public class AppDbContext(DbContextOptions options) : IdentityDbContext<IdentityUser>(options)
    {
        public DbSet<ProductsModel> Products { get; set; }

        public DbSet<CartModel> Carts { get; set; }

        public DbSet<OrderModel> Orders { get; set; }

        public DbSet<OrderDetailModel> OrderDetails { get; set; }
    }
}

