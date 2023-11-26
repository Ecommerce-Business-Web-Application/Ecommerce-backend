using EcommerceDB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : BaseController<Category, EcommerceDbContext>
    {
        public CategoryController(EcommerceDbContext context) : base(context)
        {
        }
    }
}
