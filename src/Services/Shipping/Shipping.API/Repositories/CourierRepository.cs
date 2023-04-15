using MongoDB.Driver;
using Payment.API.Models;
using Shipping.API.Data.Interfaces;
using Shipping.API.Entities;
using Shipping.API.Repositories.Interfaces;
using Stripe;

namespace Shipping.API.Repositories
{
    public class CourierRepository : ICourierRepository
    {
        private readonly IShippingContext _context;

        public CourierRepository(IShippingContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task CreateCourier(Courier courier)
        {
            await _context.Couriers.InsertOneAsync(courier);
        }

        public async Task<bool> DeleteCourier(string id)
        {
            FilterDefinition<Courier> filter = Builders<Courier>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context
                                                .Couriers
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0; ;
        }

        public async Task<Courier> GetCourier(string id)
        {
            return await _context
                                     .Couriers
                                     .Find(p => p.Id == id)
                                     .FirstOrDefaultAsync();
        }

        public async Task<ShippingResponse<Courier>> GetCouriers(int Page)
        {

            var pageSize = 6f;

            var result = await _context.Couriers
                            .Find(_ => true)
                            .SortByDescending(p => p.CreatedDate)
                            .Limit((int)pageSize)
                            .ToListAsync();

            return new ShippingResponse<Courier>
            {
                CurrentPage = Page,
                Results = result,
                TotalPages = (int)Math.Ceiling(result.Count() / pageSize)
            };
        }

        public async Task<bool> UpdateCourier(Courier courier)
        {
            var updateResult = await _context
                                        .Couriers
                                        .ReplaceOneAsync(filter: g => g.Id == courier.Id, replacement: courier);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }
    }
}
