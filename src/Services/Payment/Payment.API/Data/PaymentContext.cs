using Payment.API.Data.Interfaces;
using Payment.API.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Payment.API.Data
{
    public class PaymentContext : IPaymentContext
    {
        public PaymentContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));

            CardPayments = database.GetCollection<CardPayment>(configuration["DatabaseSettings:CollectionCardPayment"]);
            PaymentTransactions = database.GetCollection<PaymentTransaction>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));
            PaymentContextSeed.SeedData(CardPayments, PaymentTransactions);

        }

        public IMongoCollection<PaymentTransaction> PaymentTransactions { get; }
        public IMongoCollection<CardPayment> CardPayments { get; }
    }
}
