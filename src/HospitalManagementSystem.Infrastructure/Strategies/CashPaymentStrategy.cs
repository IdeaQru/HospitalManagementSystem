using HospitalManagementSystem.Domain.ValueObjects;
using HospitalManagementSystem.Application.Interfaces;
using HospitalManagementSystem.Application.Results;

namespace HospitalManagementSystem.Infrastructure.Strategies
{
    public class CashPaymentStrategy : IPaymentStrategy
    {
        public bool ValidatePayment(PaymentDetails details)
        {
            // Cash payment tidak perlu validasi khusus
            return true;
        }

        public PaymentResult ProcessPayment(decimal amount, PaymentDetails details)
        {
            if (amount <= 0)
            {
                return new PaymentResult
                {
                    IsSuccess = false,
                    Message = "Payment amount must be greater than zero"
                };
            }

            // Simulate cash payment processing
            return new PaymentResult
            {
                IsSuccess = true,
                Message = $"Cash payment of Rp {amount:N0} accepted successfully",
                TransactionId = $"CASH-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}"
            };
        }
    }
}
