using System.ComponentModel.DataAnnotations;

namespace Shipping.API.Models
{
    public class CreateLocationAddressDto
    {
        [Required]
        public string Source { get; set; }
        [Required]
        public string Destination { get; set; }

        public string CourierId { get; set; }
        public ICollection<CreatePriceDto> Prices { get; set; }
    }
}
