using System;
using System.Collections.Generic;

namespace API.DTOs
{
    public class LocalStorageOrderDto
    {  
        public DateTime OrderCreated { get; set; }
        public DateTime LastUpdate { get; set; } = DateTime.Now;
        public ICollection<OrderedProductsDto> OrderedProducts { get; set; }
    }
}