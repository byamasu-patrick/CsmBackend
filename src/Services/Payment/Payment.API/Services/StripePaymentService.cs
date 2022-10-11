using Payment.API.Entities;
using Stripe.Checkout;

namespace Payment.API.Services
{
    public class StripePaymentService : IPaymentService
    {
        public async Task<PaymentTransaction> CheckOut(PaymentTransaction paymentTransaction, string thisApiUrl, string s_wasmClientURL)
        {
            // Create a payment flow from the items in the cart.
            // Gets sent to Stripe API.

            var itemList = new List<SessionLineItemOptions>();

            for (var index=0; index < paymentTransaction.Products.Count(); index++)
            {
                itemList.Add(new SessionLineItemOptions()
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = paymentTransaction.Products[index].Price - (int)paymentTransaction.Products[index].Discount, // Price is in USD cents.
                        Currency = "USD",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = paymentTransaction.Products[index].Name,
                            Description = paymentTransaction.Products[index].Summary,
                            Images = new List<string> { paymentTransaction.Products[index].ImageUrl }
                        },
                    },
                    Quantity = paymentTransaction.Products[index].Items,
                });
            }

            var options = new SessionCreateOptions
            {
                // Stripe calls the URLs below when certain checkout events happen such as success and failure.
                SuccessUrl = $"{thisApiUrl}/checkout/success?sessionId=" + "{CHECKOUT_SESSION_ID}", // Customer paid.
                CancelUrl = s_wasmClientURL + "failed",  // Checkout cancelled.
                PaymentMethodTypes = new List<string> // Only card available in test mode?
            {
                "card"
            },
                LineItems = itemList,
                Mode = "payment" // One-time payment. Stripe supports recurring 'subscription' payments.
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            paymentTransaction.TransactionId = session.Id;

            paymentTransaction.CreatedAt = DateTime.UtcNow;

            return paymentTransaction;
        }
    }
}
