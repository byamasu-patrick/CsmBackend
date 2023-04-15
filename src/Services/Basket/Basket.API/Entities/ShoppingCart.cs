namespace Basket.API.Entities
{
    public class ShoppingCart
    {
        public string UserName { get; set; }
        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();
        public ShoppingCart()
        {
        }
        public ShoppingCart(string userName)
        {
            UserName = userName;
        }
        public decimal TotalPrice
        {
            get
            {
                decimal totalprice = 0;
                foreach (var item in Items)
                {
                    totalprice += item.Price * item.Quantity;
                }
                return totalprice;
            }
        }
        public decimal TotalWeight
        {
            get
            {
                decimal totalweight = 0;
                foreach (var item in Items)
                {
                    totalweight += item.Weight * item.Quantity;
                }
                return totalweight;
            }
        }
    }
}
