namespace Store.BasketService.Commands
{
    public interface IAddProductToBasketCommand
    {
        void Execute(string basketId, string productId);
    }
}