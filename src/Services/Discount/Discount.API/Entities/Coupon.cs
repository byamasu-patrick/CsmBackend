using Powells.CouponCode;
using System.ComponentModel.DataAnnotations;

namespace Discount.API.Entities
{
    public class Coupon
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string ProductId { get; set; }
        [Required]
        public string Headline { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string CouponCode { get; set; }
        [Required]
        public int Amount { get; set; }

    }
}
