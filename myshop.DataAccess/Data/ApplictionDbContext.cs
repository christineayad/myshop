using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using myshop.Entities;
using myshop.Entities.Models;

namespace myshop.DataAccess.Data
{
    public class ApplictionDbContext:IdentityDbContext<ApplicationUser>
    {
        public ApplictionDbContext(DbContextOptions<ApplictionDbContext> options) : base(options)
        {

        }
        public DbSet<Category>categories { get; set; }  
        public DbSet<Product>Products { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }



    }
}
