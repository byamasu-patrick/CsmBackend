using System.ComponentModel.DataAnnotations;

namespace Catalog.API.Models
{
    public class CreateReviewDto
    {
        [Required]
        public string ProductId { get; set; }
        [Required]
        public string CustomerId { get; set; }
        [Required]
        public string ReviewText { get; set; }
        [Required]
        public int Rating { get; set; }
    }
}
