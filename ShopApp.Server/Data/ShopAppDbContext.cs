using Microsoft.EntityFrameworkCore;
using ShopApp.Server.Models;

namespace ShopApp.Server.Data
{
    public class ShopAppDbContext:DbContext
    {
        public ShopAppDbContext(DbContextOptions<ShopAppDbContext> options)
            : base(options)
        {
        }
        public DbSet<Product> Products { get; set; } = null!; 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                               new Product { Id = 1, Name = "Product 1", Description = "Description 1", Image = "https://via.placeholder.com/150", Price = 100 },
                               new Product { Id = 2, Name = "Product 2", Description = "Description 2", Image = "https://via.placeholder.com/150", Price = 200 },
                               new Product { Id = 3, Name = "Product 3", Description = "Description 3", Image = "https://via.placeholder.com/150", Price = 300 });

            base.OnModelCreating(modelBuilder);
        }
    }
}
