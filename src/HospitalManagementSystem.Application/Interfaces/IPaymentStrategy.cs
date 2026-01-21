using HospitalManagementSystem.Domain.ValueObjects;
using HospitalManagementSystem.Application.Results;

namespace HospitalManagementSystem.Application.Interfaces
{
    public interface IPaymentStrategy
    {
        PaymentResult ProcessPayment(decimal amount, PaymentDetails details);
        bool ValidatePayment(PaymentDetails details);
    }
}
