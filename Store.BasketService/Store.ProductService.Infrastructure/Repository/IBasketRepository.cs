using Store.BasketService.Infrastructure.Repository.Models;

namespace Store.BasketService.Infrastructure.Repository
{
    public interface IBasketRepository
    {
        void AddItem(string basketId, string productId);
        Basket Get(string id);
        void RemoveItem(string basketId, string productId);
    }
}