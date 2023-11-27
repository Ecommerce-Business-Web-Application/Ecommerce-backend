using EcommerceData.Repositories;
using EcommerceDB;
using EcommerceDB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers
{
    [Route("api/product")]
    [ApiController]
    //public class ProductController : BaseController<Product, EcommerceDbContext>
    //{
    //    public ProductController(EcommerceDbContext context) : base(context)
    //    {
    //    }
    //}
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _product;
        public ProductController(EcommerceDbContext context)
        {
            _product = new ProductRepository(context);
        }
        [HttpGet("getproduct/{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var entity = await _product.GetByIdAsync(id);
            var test = entity.Category.Name;
            if (entity == null)
            {
                return NotFound();
            }
            return entity;
        }
    }
}
