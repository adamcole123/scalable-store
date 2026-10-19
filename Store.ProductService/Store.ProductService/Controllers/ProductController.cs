using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.ProductService.Queries;

namespace Store.ProductService.Controllers
{
    [Route("api")]
    public class ProductController(IProductProvider productProvider) : Controller
    {
        [HttpGet("products")]
        public IActionResult Get()
        {
            var products = productProvider.GetAll();

            return Ok(products);
        }
    }
}
