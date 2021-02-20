using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
        }

        public void Update(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
        }

        public void DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Photos)
                // .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Product> GetProductByNameAsync(string name)
        {
            return await _context.Products
                .Include(p => p.Photos)
                // .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(x => x.Name == name);
        }

        public async Task<Product> GetProductByPhotoIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Photos)
                .Where(p => p.Photos.Any(x => x.Id == id))
                .FirstOrDefaultAsync();
        }
    }
}