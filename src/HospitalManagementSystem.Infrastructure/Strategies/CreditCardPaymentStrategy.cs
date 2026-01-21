using HospitalManagementSystem.Domain.ValueObjects;
using HospitalManagementSystem.Application.Interfaces;
using HospitalManagementSystem.Application.Results;

namespace HospitalManagementSystem.Infrastructure.Strategies
{
    public class CreditCardPaymentStrategy : IPaymentStrategy
    {
        public bool ValidatePayment(PaymentDetails details)
        {
            if (details == null)
                return false;

            // Validasi nomor kartu (harus 16 digit)
            if (string.IsNullOrWhiteSpace(details.CardNumber) || details.CardNumber.Length != 16)
                return false;

            // Validasi CVV (harus 3 digit)
            if (string.IsNullOrWhiteSpace(details.CVV) || details.CVV.Length != 3)
                return false;

            // Validasi expiry date (tidak boleh sudah expired)
            if (details.ExpiryDate < DateTime.Now)
                return false;

            // Validasi card holder name
            if (string.IsNullOrWhiteSpace(details.CardHolderName))
                return false;

            return true;
        }

        public PaymentResult ProcessPayment(decimal amount, PaymentDetails details)
        {
            if (!ValidatePayment(details))
            {
                return new PaymentResult
                {
                    IsSuccess = false,
                    Message = "Invalid credit card details. Please check card number, CVV, and expiry date."
                };
            }

            if (amount <= 0)
            {
                return new PaymentResult
                {
                    IsSuccess = false,
                    Message = "Payment amount must be greater than zero"
                };
            }

            // Simulate credit card processing
            string maskedCardNumber = MaskCardNumber(details.CardNumber);

            return new PaymentResult
            {
                IsSuccess = true,
                Message = $"Credit card payment of Rp {amount:N0} processed successfully (Card: {maskedCardNumber})",
                TransactionId = $"CC-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}"
            };
        }

        private string MaskCardNumber(string cardNumber)
        {
            if (cardNumber.Length < 4)
                return "****";

            return $"****-****-****-{cardNumber.Substring(cardNumber.Length - 4)}";
        }
    }
}
