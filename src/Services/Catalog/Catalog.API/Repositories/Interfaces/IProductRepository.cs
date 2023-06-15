using Catalog.API.Entities;
using Catalog.API.Models;

namespace Catalog.API.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<ProductResponse<Product>> GetProducts();
        Task<Product> GetProduct(string id);
        Task<IEnumerable<Product>> GetProductByName(string name);
        Task<ProductResponse<Product>> SearchProducts(string keyword, int Page);
        Task<ProductResponse<Product>> GetProductByCategory(string categoryName, int Page);
        Task<IEnumerable<Product>> GetProductByShopOwner(string ownerId);
        Task CreateProduct(Product product);
        Task<bool> UpdateProduct(Product product);
        Task<bool> DeleteProduct(string id);
    }
}
