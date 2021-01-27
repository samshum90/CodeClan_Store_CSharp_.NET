using System.Collections.Generic;

namespace API.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public ICollection<ProductPhotoDto> Photos { get; set; }
    }
}