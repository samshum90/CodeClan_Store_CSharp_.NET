using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedProducts(DataContext context)
        {
            if (await context.Products.AnyAsync()) return;

            var productData = await System.IO.File.ReadAllTextAsync("Data/ProductSeedData.json");
            var products = JsonSerializer.Deserialize<List<Product>>(productData);
            if (products == null) return;
            
            foreach (var product in products)
            {
                context.Products.Add(product);
            }
            await context.SaveChangesAsync();
        }
    }
}