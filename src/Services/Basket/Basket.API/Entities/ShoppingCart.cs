
namespace Basket.API.Entities
{
    public class ShoppingCart
    {
        public string UserName { get; set; }
        public List<BasketProduct> Items { get; set; } = new List<BasketProduct>();
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
                    totalprice += item.TotalPrice;
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
