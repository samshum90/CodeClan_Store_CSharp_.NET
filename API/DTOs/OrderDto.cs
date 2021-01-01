using System;
using System.Collections.Generic;
using API.Entities;

namespace API.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OderDate { get; set; }
        public string status { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}