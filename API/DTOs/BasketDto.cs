using System;
using System.Collections.Generic;
using API.Entities;

namespace API.DTOs
{
    public class BasketDto
    {
        public int Id { get; set; }
        public DateTime OrderCreated { get; set; } = DateTime.Now;
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public ICollection<ProductDto> Products { get; set; }
    }
}