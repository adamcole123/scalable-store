using Store.ProductService.Infrastructure.Repository.Models;

namespace Store.ProductService.Infrastructure.Repository
{
    public interface IProductRepository
    {
        Product[] Get();
    }
}