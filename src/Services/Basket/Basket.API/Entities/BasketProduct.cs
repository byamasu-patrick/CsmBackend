using StackExchange.Redis;

namespace Basket.API.Entities
{
    public class BasketProduct
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Weight { get; set; }
        public string Color { get; set; }
    }
}
