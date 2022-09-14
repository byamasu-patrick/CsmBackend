namespace Catalog.API.Models
{
    public class ProductResponse<T>
    {
        public int CurrentPage { get; set; }
        public List<T> Results { get; set; }
        public int TotalPages { get; set; }
    }
}
