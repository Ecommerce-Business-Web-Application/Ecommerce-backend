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
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(EcommerceDbContext context) : base(context) { }

        // Implement additional methods specific to the Product entity
    }
    public interface ICategoryRepository : IRepository<Category>
    {
        // Additional methods specific to the Product entity
    }
}
