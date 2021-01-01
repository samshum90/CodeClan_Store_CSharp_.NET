using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("Orders")]
    public class order
    {
        public int Id { get; set; }
        public DateTime OderDate { get; set; }
        public string status { get; set; }
        public AppUser AppUser { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}