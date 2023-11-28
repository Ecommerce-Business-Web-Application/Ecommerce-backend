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
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _product;
        public ProductController(EcommerceDbContext context)
        {
            _product = new ProductRepository(context);
        }
        [HttpGet]
        public async Task<IEnumerable<Product>> Get()
        {
            var entity = await _product.GetAllAsync();
            if (entity == null)
            {
            }
            return entity;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(int id)
        {
            var entity = await _product.GetByIdAsync(id);
            var test = entity.Category.Name;
            if (entity == null)
            {
                return NotFound();
            }
            return entity;
        }

        [HttpPost]
        public async Task<ActionResult> Product(Product product)
        {
            await _product.AddAsync(product);
            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            var dbProduct = await _product.GetByIdAsync(id);
            if (dbProduct == null)
            {
                return NotFound();
            }
            dbProduct.ProductName = product.ProductName;
            dbProduct.ProductDescription = product.ProductDescription;
            dbProduct.categoryId = product.categoryId;
            await _product.UpdateAsync(dbProduct);
            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _product.DeleteAsync(id);
            return CreatedAtAction(nameof(Get), new { message = "Deleted" });
        }


    }
}