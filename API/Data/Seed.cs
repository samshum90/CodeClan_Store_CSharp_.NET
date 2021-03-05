using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedDb(DataContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var productData = await System.IO.File.ReadAllTextAsync("Data/ProductSeedData.json");
            var products = JsonSerializer.Deserialize<List<Product>>(productData);
            if (products == null) return;
            
            foreach (var product in products)
            {
                context.Products.Add(product);
            }

            var roles = new List<AppRole>
            {
                new AppRole{Name = "Customer"},
                new AppRole{Name = "Admin"},
                new AppRole{Name = "Moderator"},
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            var admin = new AppUser
            {
                UserName = "admin"
            };

            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" });
            
            var customer = new AppUser
            {
                UserName = "customer",
                DoB = new DateTime(1956, 07, 22),
                FirstName = "Neva",
                LastName = "Jones",
                Created = new DateTime(2021, 01, 01),
                LastActive = new DateTime(2021, 01, 01),
                Address = "123 Fake Street",
        ContactNumber = 12345678

            };
            await userManager.CreateAsync(customer, "Pa22word");
            await userManager.AddToRolesAsync(customer, new[] { "Customer" });

            await context.SaveChangesAsync();
        }
    }
}