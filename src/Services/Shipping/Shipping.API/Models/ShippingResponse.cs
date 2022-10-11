namespace Payment.API.Models
{
    public class ShippingResponse<T>
    {
        public int CurrentPage { get; set; }
        public List<T> Results { get; set; }
        public int TotalPages { get; set; }
    }
}
