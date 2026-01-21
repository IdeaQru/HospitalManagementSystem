using HospitalManagementSystem.Domain.ValueObjects;
using HospitalManagementSystem.Application.Interfaces;
using HospitalManagementSystem.Application.Results;

namespace HospitalManagementSystem.Infrastructure.Strategies
{
    public class InsurancePaymentStrategy : IPaymentStrategy
    {
        public bool ValidatePayment(PaymentDetails details)
        {
            if (details == null)
                return false;

            // Validasi policy number
            if (string.IsNullOrWhiteSpace(details.PolicyNumber))
                return false;

            // Validasi insurance provider
            if (string.IsNullOrWhiteSpace(details.InsuranceProvider))
                return false;

            // Validasi coverage limit
            if (details.CoverageLimit <= 0)
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
                    Message = "Invalid insurance details. Please check policy number and provider."
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

            // Check coverage limit
            if (amount > details.CoverageLimit)
            {
                return new PaymentResult
                {
                    IsSuccess = false,
                    Message = $"Amount Rp {amount:N0} exceeds coverage limit of Rp {details.CoverageLimit:N0}"
                };
            }

            // Simulate insurance claim processing
            return new PaymentResult
            {
                IsSuccess = true,
                Message = $"Insurance payment of Rp {amount:N0} processed successfully (Provider: {details.InsuranceProvider}, Policy: {details.PolicyNumber})",
                TransactionId = $"INS-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}"
            };
        }
    }
}
