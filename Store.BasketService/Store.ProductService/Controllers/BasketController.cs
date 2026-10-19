using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.BasketService.Commands;
using Store.BasketService.Commands.Models;
using Store.BasketService.Infrastructure.Repository.Models;
using Store.BasketService.Queries;

namespace Store.BasketService.Controllers
{
    [Route("api")]
    public class BasketController(IBasketProvider basketProvider, IRemoveProductFromBasketCommand removeProductFromBasketCommand, IAddProductToBasketCommand addProductToBasketCommand) : Controller
    {
        [HttpGet("baskets")]
        public IActionResult Get()
        {
            var Baskets = basketProvider.GetAll();

            return Ok(Baskets);
        }
        
        [HttpPut("baskets/{basketId}/add-product")]
        public IActionResult Add(string basketId, [FromBody] AddProductToBasketCommandDto dto)
        {
            addProductToBasketCommand.Execute(basketId, dto.ProductId);

            return Ok();
        }

        [HttpPut("baskets/{basketId}/remove-product")]
        public IActionResult Remove(string basketId, [FromBody] RemoveProductFromBasketCommandDto dto)
        {
            removeProductFromBasketCommand.Execute(basketId, dto.ProductId);

            return Ok();
        }
    }
}
