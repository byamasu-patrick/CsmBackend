namespace Basket.API.Entities
{
    //public enum PaymentMethod
    //{
    //    AirtelMoney,
    //    Mpamba,
    //    Card
    //}
    public class BasketCheckout
    {
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal ShippingPrice { get; set; }
        public decimal TotalWeight { get; set; }
        public List<BasketProduct> Products { get; set; }

        // BillingAddress
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string NationalId { get; set; }
        public string CourierName { get; set; }

        public string PhysicalAddress { get; set; }

        public string PaymentMethod { get; set; }

        public string OrderStatus { get; set; }
    }
}
