using System.Collections.Generic;
using API.Entities;

namespace API.DTOs
{
    public class ProductsDto
    {
        public ICollection<Product> Products { get; set; }
    }
}