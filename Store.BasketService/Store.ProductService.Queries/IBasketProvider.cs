using Store.BasketService.Queries.Models;

namespace Store.BasketService.Queries
{
    public interface IBasketProvider
    {
        BasketDto Get(string basketId);
    }
}