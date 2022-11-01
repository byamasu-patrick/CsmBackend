using Payment.API.Repositories.Interfaces;
using Payment.API.Entities;
using Payment.API.Data.Interfaces;
using MongoDB.Driver;
using Microsoft.EntityFrameworkCore;
using Payment.API.Models;
using System;
using System.Xml.Linq;
using Stripe;

namespace Payment.API.Repositories
{
    public class CardPaymentRepository : ICardPaymentRepository
    {
        private readonly IPaymentContext _context;

        public CardPaymentRepository(IPaymentContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<PaymentResponse<CardPayment>> GetCardPayments(int Page)
        {
            var pageSize = 6f;

            var filter = Builders<CardPayment>.Filter.Ne(x => x.UserId, null);

            var result = await _context.CardPayments
                            .Find(filter)
                            .SortByDescending(p => p.CreatedAt)
                            .Limit((int)pageSize)
                            .ToListAsync();

            return new PaymentResponse<CardPayment>
            {
                CurrentPage = Page,
                Results = result,
                TotalPages = (int)Math.Ceiling(result.Count() / pageSize)
            };
        }

        public async Task<CardPayment> GetCardPayment(string id)
        {
            return await _context
                           .CardPayments
                           .Find(p => p.Id == id)
                           .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CardPayment>> GetCardPaymentUserId(string id)
        {
            FilterDefinition<CardPayment> filter = Builders<CardPayment>.Filter.Eq(card => card.UserId, id);


            return await _context
                            .CardPayments
                            .Find(filter)
                            .ToListAsync();
        }

        public async Task CreateCardPayment(CardPayment payment)
        {
            await _context.CardPayments.InsertOneAsync(payment);
        }

        public async Task<bool> UpdateCardPayment(CardPayment payment)
        {
            var updateResult = await _context
                                        .CardPayments
                                        .ReplaceOneAsync(filter: g => g.Id == payment.Id, replacement: payment);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> DeleteCardPayment(string id)
        {
            FilterDefinition<CardPayment> filter = Builders<CardPayment>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context
                                                .CardPayments
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }
    }
}
