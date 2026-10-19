using Store.BasketService.Infrastructure.Repository;

namespace Store.BasketService.Commands
{
    public class RemoveProductFromBasketCommand(IBasketRepository basketRepository) : IRemoveProductFromBasketCommand
    {
        public void Execute(string basketId, string productId)
        {
            basketRepository.RemoveItem(basketId, productId);
        }
    }
}
