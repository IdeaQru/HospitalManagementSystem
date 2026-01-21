using HospitalManagementSystem.Domain.ValueObjects;
using HospitalManagementSystem.Application.Interfaces;
using HospitalManagementSystem.Application.Results;

namespace HospitalManagementSystem.Infrastructure.Strategies
{
    public class BPJSPaymentStrategy : IPaymentStrategy
    {
        // Tarif maksimal per kelas BPJS
        private readonly Dictionary<int, decimal> _classCoverageLimit = new Dictionary<int, decimal>
        {
            { 1, 1000000 }, // Kelas 1: Rp 1.000.000
            { 2, 750000 },  // Kelas 2: Rp 750.000
            { 3, 500000 }   // Kelas 3: Rp 500.000
        };

        public bool ValidatePayment(PaymentDetails details)
        {
            if (details == null)
                return false;

            // Validasi BPJS card number (13 digit)
            if (string.IsNullOrWhiteSpace(details.BPJSCardNumber) || details.BPJSCardNumber.Length != 13)
                return false;

            // Validasi kelas perawatan (1, 2, atau 3)
            if (details.TreatmentClass < 1 || details.TreatmentClass > 3)
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
                    Message = "Invalid BPJS details. Card number must be 13 digits and treatment class must be 1, 2, or 3."
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

            // Check coverage limit berdasarkan kelas
            decimal coverageLimit = _classCoverageLimit[details.TreatmentClass];

            if (amount > coverageLimit)
            {
                decimal excess = amount - coverageLimit;
                return new PaymentResult
                {
                    IsSuccess = false,
                    Message = $"Amount Rp {amount:N0} exceeds BPJS Class {details.TreatmentClass} coverage limit of Rp {coverageLimit:N0}. Excess amount: Rp {excess:N0}"
                };
            }

            // Simulate BPJS processing
            string maskedCardNumber = MaskBPJSNumber(details.BPJSCardNumber);

            return new PaymentResult
            {
                IsSuccess = true,
                Message = $"BPJS payment of Rp {amount:N0} processed successfully (Class {details.TreatmentClass}, Card: {maskedCardNumber})",
                TransactionId = $"BPJS-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}"
            };
        }

        private string MaskBPJSNumber(string cardNumber)
        {
            if (cardNumber.Length < 4)
                return "****";

            return $"****-****-*{cardNumber.Substring(cardNumber.Length - 4)}";
        }
    }
}
