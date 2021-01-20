using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("Orders")]
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderCreated { get; set; } = DateTime.Now;
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public AppUser AppUser { get; set; }
        public int AppUserId { get; set; }
        public ICollection<Product> Products { get; set; }  = new List<Product>();
    }
}