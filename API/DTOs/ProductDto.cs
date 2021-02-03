using System.Collections.Generic;

namespace API.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProductPrice { get; set; }
        public string SalePrice { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int Stock { get; set; }
        public bool Highlight { get; set; }
        public ICollection<ProductPhotoDto> Photos { get; set; }
    }
}