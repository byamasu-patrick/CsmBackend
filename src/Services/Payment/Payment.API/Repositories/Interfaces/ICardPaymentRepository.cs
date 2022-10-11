using Payment.API.Entities;
using Payment.API.Models;

namespace Payment.API.Repositories.Interfaces
{
    public interface ICardPaymentRepository
    {
        Task<PaymentResponse<CardPayment>> GetCardPayments(int Page);
        Task<CardPayment> GetCardPayment(string id);
        Task<IEnumerable<CardPayment>> GetCardPaymentUserId(string id);
        Task CreateCardPayment(CardPayment payment);
        Task<bool> UpdateCardPayment(CardPayment payment);
        Task<bool> DeleteCardPayment(string id);
    }
}
