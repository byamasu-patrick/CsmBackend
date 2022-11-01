using System;
using System.ComponentModel.DataAnnotations;

namespace Discount.Grpc.Entities
{
    public class Coupon
    {
        [Required]
        [Key]
        public int Id { get; set; }
        public int Amount { get; set; }
        public string ProductName { get; set; }
        public string ProductId { get; set; }
        public string Headline { get; set; }
        public string Description { get; set; }
        public string CouponCode { get; set; }

    }
}
