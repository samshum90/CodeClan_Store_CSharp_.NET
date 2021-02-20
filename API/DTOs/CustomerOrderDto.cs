using System;
using System.Collections.Generic;

namespace API.DTOs
{
    public class CustomerOrderDto
    {
        public int Id { get; set; }
        public DateTime OrderCreated { get; set; } = DateTime.Now;
        public DateTime OrderDate { get; set; } 
        public string Status { get; set; } 
        public ICollection<OrderedProductsDto> OrderedProducts { get; set; }
    }
}