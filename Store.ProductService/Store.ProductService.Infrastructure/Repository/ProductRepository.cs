using Store.ProductService.Infrastructure.Persistence;
using Store.ProductService.Infrastructure.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.ProductService.Infrastructure.Repository
{
    public class ProductRepository(AppDbContext context) : IProductRepository
    {
        public Product[] Get()
        {
            return [
                new Product(){
                    ProductId = "Test",
                    Name = "Test Product",
                    Price = 20.00
                },
                new Product(){
                    ProductId = "Test 2",
                    Name = "Test Daria 2",
                    Price = 20.00
                }
            ];
            //return context.Products.ToArray();
        }
    }
}
