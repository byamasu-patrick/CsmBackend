using MongoDB.Driver;
using Payment.API.Models;
using Shipping.API.Data.Interfaces;
using Shipping.API.Entities;
using Shipping.API.Repositories.Interfaces;
using Stripe;

namespace Shipping.API.Repositories
{
    public class PriceRepository : IPriceRepository
    {
        private readonly IShippingContext _context;

        public PriceRepository(IShippingContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task CreatePrice(Prices price)
        {
            await _context.Prices.InsertOneAsync(price);
        }

        public async Task<bool> DeletePrice(string id)
        {
            FilterDefinition<Prices> filter = Builders<Prices>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context
                                                .Prices
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0; ;
        }

        public async Task<Prices> GetPrice(string id)
        {
            return await _context
                                     .Prices
                                     .Find(p => p.Id == id)
                                     .FirstOrDefaultAsync();
        }

        public async Task<ShippingResponse<Prices>> GetPrices(int Page)
        {

            var pageSize = 6f;

            var result = await _context.Prices
                            .Find(_ => true)
                            .SortByDescending(p => p.FromKg)
                            .Limit((int)pageSize)
                            .ToListAsync();

            return new ShippingResponse<Prices>
            {
                CurrentPage = Page,
                Results = result,
                TotalPages = (int)Math.Ceiling(result.Count() / pageSize)
            };
        }

        public async Task<bool> UpdatePrice(Prices price)
        {
            var updateResult = await _context
                                        .Prices
                                        .ReplaceOneAsync(filter: g => g.Id == price.Id, replacement: price);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }
    }
}
