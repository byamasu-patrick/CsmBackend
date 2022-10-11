using Payment.API.Entities;

namespace Payment.API.Services
{
    public interface IPaymentService
    {
        Task<PaymentTransaction> CheckOut(PaymentTransaction paymentTransaction, string thisApiUrl, string s_wasmClientURL);
    }
}
