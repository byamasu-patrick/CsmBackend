using Shipping.API.Entities;
using System.ComponentModel.DataAnnotations;

namespace Shipping.API.Models
{
    public class CreatePriceDto
    {
        public string KillogramRange { get; set; }
        [Required]
        public string price { get; set; }
        public string LocationId { get; set; }

    }
}
