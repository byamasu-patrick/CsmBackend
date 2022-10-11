using Payment.API.Entities;
using MongoDB.Driver;

namespace Payment.API.Data.Interfaces
{
    public interface IPaymentContext
    {
        IMongoCollection<CardPayment> CardPayments { get; }
        IMongoCollection<PaymentTransaction> PaymentTransactions { get; }
    }
}
