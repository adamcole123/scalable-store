namespace Store.BasketService.Commands
{
    public interface IRemoveProductFromBasketCommand
    {
        void Execute(string basketId, string productId);
    }
}