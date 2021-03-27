using System;
using System.Collections.Generic;
using API.Data.Migrations;

namespace API.DTOs
{
    public class AdminOrderDto
    {
        public int Id { get; set; }
        public DateTime OrderCreated { get; set; }
        public DateTime LastUpdate { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public MemberDto AppUser { get; set; }
        public virtual ICollection<OrderedProductsDto> OrderedProducts { get; set; } 
    }
}