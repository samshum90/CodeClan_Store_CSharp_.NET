using System.Collections.Generic;

namespace API.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProductPrice { get; set; }
        public string SalePrice { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int Stock { get; set; }
        public bool Highlight { get; set; }
        public ICollection<ProductPhoto> Photos { get; set; }
        public virtual ICollection<OrderedProducts> OrderedProducts { get; set; }
    }
}