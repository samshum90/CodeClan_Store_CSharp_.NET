using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public ProductRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public void Update(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
        }
        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _context.Products
                .ToListAsync();
        }
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product> GetProductByNameAsync(string name)
        {
            return await _context.Products
                .SingleOrDefaultAsync(x => x.Name == name);
        }
        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
        }
        public void DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
        }
    }
}