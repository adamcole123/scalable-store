using Store.ProductService.Infrastructure.Repository;
using Store.ProductService.Queries.Models;

namespace Store.ProductService.Queries
{
    public class ProductProvider(IProductRepository productRepository) : IProductProvider
    {
        public IEnumerable<ProductDto> GetAll()
        {
            return productRepository.Get().Select(x => new ProductDto {
                Id = x.ProductId,
                Name = x.Name,
                Price = x.Price,
            });
        }
    }
}
