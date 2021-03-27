using System;
using System.Collections.Generic;
using API.Entities;

namespace API.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderCreated { get; set; } = DateTime.Now;
        public DateTime LastUpdate { get; set; } = DateTime.Now;
        public DateTime OrderDate { get; set; } 
        public string Status { get; set; } 
        public int AppUserId { get; set; }
        public ICollection<OrderedProductsDto> OrderedProducts { get; set; }
    }
}