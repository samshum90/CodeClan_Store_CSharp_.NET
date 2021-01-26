using System.Collections.Generic;

namespace API.DTOs
{
    public class EditOrderDto
    {
        public string Status { get; set; } 
        public ICollection<ProductDto> Products { get; set; }
    }
}