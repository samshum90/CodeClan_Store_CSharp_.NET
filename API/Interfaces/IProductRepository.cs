using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IProductRepository
    {
        void AddProduct(Product product);
        void Update(Product product);
        void DeleteProduct(Product product);
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> GetProductByNameAsync(string name);
        Task<Product> GetProductByPhotoIdAsync(int id);
    }
}