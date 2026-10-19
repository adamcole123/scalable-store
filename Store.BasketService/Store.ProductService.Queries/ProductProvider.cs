using Store.BasketService.Infrastructure.Repository;
using Store.BasketService.Queries.Models;

namespace Store.BasketService.Queries
{
    public class BasketProvider(IBasketRepository BasketRepository) : IBasketProvider
    {
        public IEnumerable<BasketDto> GetAll()
        {
            return BasketRepository.Get().Select(x => new BasketDto {
                Id = x.BasketId,
                Name = x.Name,
                Price = x.Price,
            });
        }
    }
}
