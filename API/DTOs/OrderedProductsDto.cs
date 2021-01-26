namespace API.DTOs
{
    public class OrderedProductsDto
    {
        public virtual ProductDto Product { get; set; }
        public int Quantity { get; set; }
    }
}