using EcommerceDB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : BaseController<Product, EcommerceDbContext>
    {
        public ProductController(EcommerceDbContext context) : base(context)
        {
        }
    }
    }
