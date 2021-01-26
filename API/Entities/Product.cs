using System.Collections.Generic;

namespace API.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public ICollection<ProductPhoto> Photos { get; set; }
        public virtual ICollection<OrderedProducts> OrderedProducts { get; set; }
    }
}