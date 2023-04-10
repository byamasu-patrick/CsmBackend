using MongoDB.Driver;
using Payment.API.Models;
using Shipping.API.Data.Interfaces;
using Shipping.API.Entities;
using Shipping.API.Repositories.Interfaces;
using Stripe;

namespace Shipping.API.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly IShippingContext _context;

        public LocationRepository(IShippingContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task CreateLocation(LocationAddress location)
        {
            await _context.Locations.InsertOneAsync(location);
        }

        public async Task<bool> DeleteLocation(string id)
        {
            FilterDefinition<LocationAddress> filter = Builders<LocationAddress>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context
                                                .Locations
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0; ;
        }

        public async Task<LocationAddress> GetLocation(string id)
        {
            return await _context
                                     .Locations
                                     .Find(p => p.Id == id)
                                     .FirstOrDefaultAsync();
        }

        public async Task<ShippingResponse<LocationAddress>> GetLocations(int Page)
        {

            var pageSize = 6f;

            var result = await _context.Locations
                            .Find(_ => true)
                            .SortByDescending(p => p.Source)
                            .Limit((int)pageSize)
                            .ToListAsync();

            return new ShippingResponse<LocationAddress>
            {
                CurrentPage = Page,
                Results = result,
                TotalPages = (int)Math.Ceiling(result.Count() / pageSize)
            };
        }

        public async Task<bool> UpdateLocation(LocationAddress location)
        {
            var updateResult = await _context
                                        .Locations
                                        .ReplaceOneAsync(filter: g => g.Id == location.Id, replacement: location);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }
    }
}
