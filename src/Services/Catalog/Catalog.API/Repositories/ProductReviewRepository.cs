using Catalog.API.Data.Interfaces;
using Catalog.API.Entities;
using Catalog.API.Models;
using Catalog.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System.Xml.Linq;

namespace Catalog.API.Repositories
{
    public class ProductReviewRepository : IProductReviewRepository
    {
        private readonly ICatalogContext _context;
        public ProductReviewRepository(ICatalogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task CreateReview(ProductReview review)
        {
            await _context.ProductReview.InsertOneAsync(review);
        }

        public async Task<bool> DeleteReview(string reviewId)
        {
            FilterDefinition<ProductReview> filter = Builders<ProductReview>.Filter.Eq(p => p.Id, reviewId);

            DeleteResult deleteResult = await _context
                                                .ProductReview
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<ProductReview>> GetReview(string id)
        {
            FilterDefinition<ProductReview> filter = Builders<ProductReview>.Filter.Eq(p => p.Id, id);
            var result = await _context
                            .ProductReview
                            .Find(filter)
                            .ToListAsync();
            return result;
        }

        public async Task<ProductResponse<ProductReview>> GetProductReviewByProductId(string productId, int Page)
        {
            var pageSize = 6f;

            FilterDefinition<ProductReview> filter = Builders<ProductReview>.Filter.Eq(review => review.ProductId, productId);

            var TotalPages = _context.ProductReview.Count(filter);

            var result = await _context.ProductReview
                            .Find(filter)
                            .Skip((Page - 1) * (int)pageSize)
                            .Limit((int)pageSize)
                            .ToListAsync();

            return new ProductResponse<ProductReview>
            {
                CurrentPage = Page,
                Results = result,
                TotalPages = (int)Math.Ceiling(TotalPages / pageSize)
            };
        }

        public async Task<bool> UpdateReview(ProductReview review)
        {
            var updateResult = await _context
                                       .ProductReview
                                       .ReplaceOneAsync(filter: g => g.Id == review.Id, replacement: review);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }
    }
}
