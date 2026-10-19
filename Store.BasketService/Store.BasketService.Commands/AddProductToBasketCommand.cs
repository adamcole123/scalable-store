using Store.BasketService.Infrastructure.Repository;

namespace Store.BasketService.Commands
{
    public class AddProductToBasketCommand(IBasketRepository basketRepository) : IAddProductToBasketCommand
    {
        public void Execute(string basketId, string productId)
        {
            basketRepository.AddItem(basketId, productId);
        }
    }
}
