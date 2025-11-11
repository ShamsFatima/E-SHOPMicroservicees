using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.ValueObjects
{
    public record Payment
    {
        public string? CardName { get; } = default!;
        public string? CardNumber { get; }=default!;
        public string Expiration { get; } = default!;
        public string CVV {  get; } = default!; 
        public int  PaymentMethod { get; } = default!;
        public Payment()
        {
        }
        private Payment(string cardName, string cardNumber, string expiration, string cvv, int paymentMethod)
        {
            CardName = cardName;
            CardNumber = cardNumber;
            Expiration = expiration;
            CVV = cvv;
            PaymentMethod = paymentMethod;
        }
        public static Payment Of(string cardName, string cardNumber, string expiration, string cvv, int paymentMethod)
        {
            ArgumentNullException.ThrowIfNull(cardName, nameof(cardName));
            ArgumentNullException.ThrowIfNull(cardNumber, nameof(cardNumber));
            ArgumentNullException.ThrowIfNull(expiration, nameof(expiration));
            ArgumentNullException.ThrowIfNull(cvv, nameof(cvv));
            return new Payment(cardName, cardNumber, expiration, cvv, paymentMethod);
        }   
    }
}
