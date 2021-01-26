using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("ProductPhotos")]
    public class ProductPhoto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
    }
}