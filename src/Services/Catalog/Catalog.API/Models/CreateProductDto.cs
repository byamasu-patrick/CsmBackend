using System.ComponentModel.DataAnnotations;

namespace Catalog.API.Models
{
    public class CreateProductDto {      // 
     // Data Transfer Object
        [Required]
        public string Name { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string Summary { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ImageFile { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public decimal Weight { get; set; }
        [Required]
        public int ItemsInStock { get; set; }
        [Required]
        public string UserId { get; set; }
    }
}
