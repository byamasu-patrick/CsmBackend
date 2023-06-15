namespace Basket.API.Entities
{
    public class ShoppingCartItem
    {
        public int Quantity { get; set; }
        public string Color { get; set; }
        public decimal InitialPrice { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal TotalPrice
        {
            get
            {
               return InitialPrice * Quantity;
            }
        }
    }
}
