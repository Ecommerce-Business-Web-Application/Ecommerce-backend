using Microsoft.EntityFrameworkCore;
using EcommerceDB.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace EcommerceDB
{
    public class EcommerceDbContext : IdentityDbContext<IdentityUser>
    {
        public EcommerceDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}