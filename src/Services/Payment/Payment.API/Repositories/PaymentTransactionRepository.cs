using Payment.API.Data.Interfaces;
using Payment.API.Entities;
using Payment.API.Models;
using Payment.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System.Xml.Linq;

namespace Payment.API.Repositories
{
    public class PaymentTransactionRepository : IPaymentTransactionRepository
    {
        private readonly IPaymentContext _context;
        public PaymentTransactionRepository(IPaymentContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> CancelPaymentTransaction(string transanctionId)
        {
            FilterDefinition<PaymentTransaction> filter = Builders<PaymentTransaction>.Filter.Eq(p => p.TransactionId, transanctionId);

            DeleteResult deleteResult = await _context
                                                .PaymentTransactions
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }

        public async Task CheckoutPaymentTransaction(PaymentTransaction payment)
        {
            await _context.PaymentTransactions.InsertOneAsync(payment);
        }

        public async Task<IEnumerable<PaymentTransaction>> GetCardPaymentTransactionUserId(string id)
        {
            FilterDefinition<PaymentTransaction> filter = Builders<PaymentTransaction>.Filter.Eq(p => p.UserId, id);

            return await _context
                            .PaymentTransactions
                            .Find(filter)
                            .ToListAsync();
        }

        public async Task<PaymentTransaction> GetPaymentTransaction(string id)
        {
            return await _context
                           .PaymentTransactions
                           .Find(p => p.Id == id)
                           .FirstOrDefaultAsync();
        }

        public async Task<PaymentResponse<PaymentTransaction>> GetPaymentTransactions(int Page)
        {
            var pageSize = 6f;

            var filter = Builders<PaymentTransaction>.Filter.Ne(x => x.UserId, null);

            var result = await _context.PaymentTransactions
                            .Find(filter)
                            .SortByDescending(p => p.CreatedAt)
                            .Limit((int)pageSize)
                            .ToListAsync();

            return new PaymentResponse<PaymentTransaction>
            {
                CurrentPage = Page,
                Results = result,
                TotalPages = (int)Math.Ceiling(result.Count() / pageSize)
            };
        }

    }
}
