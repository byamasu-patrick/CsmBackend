using Shipping.API.Entities;
using System.ComponentModel.DataAnnotations;

namespace Shipping.API.Models
{
    public class CreatePriceDto
    {
        public decimal FromKg { get; set; }

        public decimal ToKg { get; set; }
        [Required]
        public decimal price { get; set; }
        public string LocationId { get; set; }

    }
}
