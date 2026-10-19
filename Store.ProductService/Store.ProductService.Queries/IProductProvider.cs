using Store.ProductService.Queries.Models;

namespace Store.ProductService.Queries
{
    public interface IProductProvider
    {
        IEnumerable<ProductDto> GetAll();
    }
}