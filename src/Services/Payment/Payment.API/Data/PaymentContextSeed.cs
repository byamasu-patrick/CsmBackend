using Payment.API.Entities;
using MongoDB.Driver;
using Payment.API.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Payment.API.Data
{
    public class PaymentContextSeed
    {
        public static void SeedData(IMongoCollection<CardPayment> cardPaymentCollection, IMongoCollection<PaymentTransaction> paymentTransactionCollection)
        {
            
            //Console.WriteLine("Arrived");
            bool existProduct = cardPaymentCollection.Find(p => true).Any();
            if (!existProduct)
            {
                cardPaymentCollection.InsertManyAsync(GetPreconfiguredCardPayment());
                paymentTransactionCollection.InsertOneAsync(GetPreconfiguredPaymentTransaction());
            }
        }
        private static IEnumerable<CardPayment> GetPreconfiguredCardPayment()
        {
            return new List<CardPayment>()
            {
               new CardPayment()
               {
                   CardName = "BYAMASU PAUL",
                   CardNumber = "8824-8835-7122-7291",
                   CVV = "520",
                   UserId = "ptrckbyamaasu@gmail.com",
                   Default = false,
                   Expiration = "03/23",
                   CreatedAt = DateTime.UtcNow
               }
            };
        }

        private static PaymentTransaction GetPreconfiguredPaymentTransaction()
        {
            return new PaymentTransaction()
            {
                 IsApproved = true,
                 PaymentMethod = "Card-Payment",
                 Products =  null,
                 TotalAmount = 87921,
                 TransactionId = "bg872218nghs",
                 UserId = "ptrckbyamasu@gmailcom",
                 CreatedAt = DateTime.UtcNow               
            };
        }
    }
}
