using Store.BasketService.Infrastructure.Repository;
using Store.BasketService.Queries.Models;

namespace Store.BasketService.Queries
{
    public class BasketProvider(IBasketRepository BasketRepository) : IBasketProvider
    {
        public BasketDto Get(string basketId)
        {
            var basket = BasketRepository.Get(basketId);

            return new BasketDto()
            {
                Id = basket.BasketId,
                Items = basket.Items,
            };
        }
    }
}
