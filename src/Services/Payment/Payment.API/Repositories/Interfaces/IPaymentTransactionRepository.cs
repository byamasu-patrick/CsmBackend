using Payment.API.Entities;
using Payment.API.Models;

namespace Payment.API.Repositories.Interfaces
{
    public interface IPaymentTransactionRepository
    {
        Task<PaymentResponse<PaymentTransaction>> GetPaymentTransactions(int Page);
        Task<PaymentTransaction> GetPaymentTransaction(string id);
        Task<IEnumerable<PaymentTransaction>> GetCardPaymentTransactionUserId(string id);
        Task CheckoutPaymentTransaction(PaymentTransaction payment);
        Task<bool> CancelPaymentTransaction(string transactionId);
    }
}
