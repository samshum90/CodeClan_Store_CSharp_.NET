using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IProductRepository
    {
        void Update(Product product);
        Task<IEnumerable<ProductDto>> GetProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> GetProductByNameAsync(string name);
        void AddProduct(Product product);
        void DeleteProduct(Product product);
        Task<Product> GetProductByPhotoIdAsync(int id);
    }
}