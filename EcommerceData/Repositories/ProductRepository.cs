using EcommerceDB;
using EcommerceDB.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceData.Repositories
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(EcommerceDbContext context) : base(context) { }

        // Implement additional methods specific to the Product entity
    }
    public interface IProductRepository : IRepository<Product>
    {
        // Additional methods specific to the Product entity
    }
}
