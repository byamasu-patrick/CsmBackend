using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Payment.API.Models;
using Shipping.API.Data.Interfaces;
using Shipping.API.Entities;
using Shipping.API.Repositories.Interfaces;

namespace Shipping.API.Repositories
{
    public class ShippingAddressRepository : IShippingAddressRepository
    {
        private readonly IShippingContext _context;

        public ShippingAddressRepository(IShippingContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task CreateShippingAddress(ShippingAddress shipping)
        {
            await _context.ShippingAddresses.InsertOneAsync(shipping);
        }

        public async Task<bool> DeleteShippingAddress(string id)
        {
            FilterDefinition<ShippingAddress> filter = Builders<ShippingAddress>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context
                                                .ShippingAddresses
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0; ;
        }

        public async Task<ShippingAddress> GetShippingAddress(string id)
        {
            return await _context
                           .ShippingAddresses
                           .Find(p => p.Id == id)
                           .FirstOrDefaultAsync();
        }

        public async Task<ShippingResponse<ShippingAddress>> GetShippingAddresses(int Page)
        {

            var pageSize = 6f;

            var result = await _context.ShippingAddresses
                            .Find(_ => true)
                            .SortByDescending(p => p.CreatedAt)
                            .Limit((int)pageSize)
                            .ToListAsync();

            return new ShippingResponse<ShippingAddress>
            {
                CurrentPage = Page,
                Results = result,
                TotalPages = (int)Math.Ceiling(result.Count() / pageSize)
            };
        }

        public async Task<IEnumerable<ShippingAddress>> GetShippingAddressUserId(string id)
        {
            FilterDefinition<ShippingAddress> filter = Builders<ShippingAddress>.Filter.Eq(p => p.CustomerId, id);

            return await _context
                            .ShippingAddresses
                            .Find(filter)
                            .ToListAsync();
        }

        public async Task<bool> UpdateShippingAddress(ShippingAddress shipping)
        {
            var updateResult = await _context
                                        .ShippingAddresses
                                        .ReplaceOneAsync(filter: g => g.Id == shipping.Id, replacement: shipping);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }
    }
}
