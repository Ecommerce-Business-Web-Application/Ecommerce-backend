using EcommerceData.Repositories;
using EcommerceDB;
using EcommerceDB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _category;
        public CategoryController(EcommerceDbContext context)
        {
            _category = new CategoryRepository(context);
        }

        [HttpGet]
        public async Task<IEnumerable<Category>> Get()
        {
            var entity = await _category.GetAllAsync();
            if (entity == null)
            {
                return Enumerable.Empty<Category>();
            }
            return entity;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> Get(int id)
        {
            var entity = await _category.GetByIdAsync(id);
            if(entity == null)
            {
                return NotFound();
            }
            return entity;
        }

        [HttpPost]
        public async Task<ActionResult> Product(Category category)
        {
            await _category.AddAsync(category);
            return CreatedAtAction(nameof(Get), new { id = category.Id }, category);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Category category)
        {
            var dbCategory = await _category.GetByIdAsync(id);
            if (dbCategory == null)
            {
                return NotFound();
            }
            dbCategory.Name = category.Name;
            dbCategory.parentCategoryId = category.parentCategoryId;
            await _category.UpdateAsync(dbCategory);
            return CreatedAtAction(nameof(Get), new { id = category.Id }, category);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _category.DeleteAsync(id);
            return CreatedAtAction(nameof(Get), new { message = "Deleted" });
        }

    }
}
