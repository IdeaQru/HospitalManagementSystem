namespace HospitalManagementSystem.Domain.Exceptions
{
    public class InvalidPaymentException : DomainException
    {
        public InvalidPaymentException(string message) : base(message)
        {
        }

        public static InvalidPaymentException InvalidAmount()
        {
            return new InvalidPaymentException("Payment amount must be greater than zero");
        }

        public static InvalidPaymentException PaymentAlreadyCompleted()
        {
            return new InvalidPaymentException("Payment has already been completed");
        }

        public static InvalidPaymentException InvalidCardDetails()
        {
            return new InvalidPaymentException("Invalid credit card details");
        }

        public static InvalidPaymentException InsufficientCoverage()
        {
            return new InvalidPaymentException("Insurance coverage is insufficient");
        }
    }
}
