using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Payment.API.Models;
using Shipping.API.Data.Interfaces;
using Shipping.API.Entities;
using Shipping.API.Repositories.Interfaces;

namespace Shipping.API.Repositories
{
    public class ShippingMethodRepository : IShippingMethodRepository
    {
        private readonly IShippingContext _context;

        public ShippingMethodRepository(IShippingContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task CreateShippingMethod(ShippingMethods shipping)
        {
            await _context.ShippingMethods.InsertOneAsync(shipping);
        }

        public async Task<bool> DeleteShippingMethod(string id)
        {
            FilterDefinition<ShippingMethods> filter = Builders<ShippingMethods>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context
                                                .ShippingMethods
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }

        public async Task<ShippingMethods> GetShippingMethod(string id)
        {
            return await _context
                           .ShippingMethods
                           .Find(p => p.Id == id)
                           .FirstOrDefaultAsync();
        }

        public async Task<ShippingResponse<ShippingMethods>> GetShippingMethods(int Page)
        {

            var pageSize = 6f;

            var result = await _context.ShippingMethods
                            .Find(_ => true)
                            .SortByDescending(p => p.CreatedAt)
                            .Limit((int)pageSize)
                            .ToListAsync();

            return new ShippingResponse<ShippingMethods>
            {
                CurrentPage = Page,
                Results = result,
                TotalPages = (int)Math.Ceiling(result.Count() / pageSize)
            };
        }

        public async Task<IEnumerable<ShippingMethods>> GetShippingMethodUserId(string id)
        {
            FilterDefinition<ShippingMethods> filter = Builders<ShippingMethods>.Filter.ElemMatch(p => p.Id, id);

            return await _context
                            .ShippingMethods
                            .Find(filter)
                            .ToListAsync();
        }

        public async Task<bool> UpdateShippingMethod(ShippingMethods shipping)
        {
            var updateResult = await _context
                                        .ShippingMethods
                                        .ReplaceOneAsync(filter: g => g.Id == shipping.Id, replacement: shipping);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }
    }
}
