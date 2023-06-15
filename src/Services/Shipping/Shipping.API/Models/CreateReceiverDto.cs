using Shipping.API.Entities;
using System.ComponentModel.DataAnnotations;

namespace Shipping.API.Models
{
    public class CreateReceiverDto
    {
        [Required]
        public string UserName { get; set; }
        public string Name { get; set; }
        [Required]
        public string Contact { get; set; }
        [Required]
        public string NationalId { get; set; }
        [Required]
        public string Killograms { get; set; }

        public string Courier { get; set; }

        public string Location { get; set; }

        public string Price { get; set; }
    }
}
