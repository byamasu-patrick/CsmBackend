using MongoDB.Driver;
using Payment.API.Models;
using Shipping.API.Data.Interfaces;
using Shipping.API.Entities;
using Shipping.API.Repositories.Interfaces;
using Stripe;

namespace Shipping.API.Repositories
{
    public class ReceiverRepository : IReceiverRepository
    {
        private readonly IShippingContext _context;

        public ReceiverRepository(IShippingContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task CreateReceiver(Receiver receiver)
        {
            await _context.Receivers.InsertOneAsync(receiver);
        }

        public async Task<bool> DeleteReceiver(string id)
        {
            FilterDefinition<Receiver> filter = Builders<Receiver>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context
                                                .Receivers
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0; ;
        }

        public async Task<Receiver> GetReceiver(string id)
        {
            return await _context
                                     .Receivers
                                     .Find(p => p.Id == id)
                                     .FirstOrDefaultAsync();
        }

        public async Task<ShippingResponse<Receiver>> GetReceivers(int Page)
        {

            var pageSize = 6f;

            var result = await _context.Receivers
                            .Find(_ => true)
                            .SortByDescending(p => p.UserName)
                            .Limit((int)pageSize)
                            .ToListAsync();

            return new ShippingResponse<Receiver>
            {
                CurrentPage = Page,
                Results = result,
                TotalPages = (int)Math.Ceiling(result.Count() / pageSize)
            };
        }

        public async Task<bool> UpdateReceiver(Receiver receiver)
        {
            var updateResult = await _context
                                        .Receivers
                                        .ReplaceOneAsync(filter: g => g.Id == receiver.Id, replacement: receiver);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }
    }
}
