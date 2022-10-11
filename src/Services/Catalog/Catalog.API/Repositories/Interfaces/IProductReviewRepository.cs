using Catalog.API.Entities;
using Catalog.API.Models;

namespace Catalog.API.Repositories.Interfaces
{
    public interface IProductReviewRepository
    {
        Task<IEnumerable<ProductReview>> GetReview(string id);
        Task<ProductResponse<ProductReview>> GetProductReviewByProductId(string productId, int Page);
        Task CreateReview(ProductReview review);
        Task<bool> UpdateReview(ProductReview review);
        Task<bool> DeleteReview(string reviewId);
    }
}
