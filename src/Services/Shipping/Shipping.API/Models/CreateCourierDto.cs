using System.ComponentModel.DataAnnotations;

namespace Shipping.API.Models
{
    public class CreateCourierDto
    {
        public string Name { get; set; }
        [Required]
        public string Contact1 { get; set; }
        [Required]
        public string Contact2 { get; set; }
        [Required]
        public string HeadOfficeLocation { get; set; }

        public ICollection<CreateLocationAddressDto> LocationAddresses { get; set; }
    }
}
