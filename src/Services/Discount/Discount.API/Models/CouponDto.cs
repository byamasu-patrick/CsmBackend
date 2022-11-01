namespace Discount.API.Models
{
    public class CouponDto
    {
        public string ProductName { get; set; }
        public string ProductId { get; set; }
        public string Headline { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
    }
}
