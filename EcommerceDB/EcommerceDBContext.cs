using Microsoft.EntityFrameworkCore;
using EcommerceDB.Models;

namespace Ecommerce
{
    public class EcommerceDbContext : DbContext
    {
        public EcommerceDbContext(DbContextOptions options) : base(options)
        {
        }

        DbSet<Category> Categories { get; set; }
        DbSet<Product> Products { get; set; }
    }
}