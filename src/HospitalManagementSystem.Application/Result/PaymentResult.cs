namespace HospitalManagementSystem.Application.Results
{
    public class PaymentResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string TransactionId { get; set; }
        public DateTime ProcessedAt { get; set; }

        public PaymentResult()
        {
            ProcessedAt = DateTime.Now;
        }

        public static PaymentResult Success(string transactionId, string message = "Payment processed successfully")
        {
            return new PaymentResult
            {
                IsSuccess = true,
                Message = message,
                TransactionId = transactionId
            };
        }

        public static PaymentResult Failure(string message)
        {
            return new PaymentResult
            {
                IsSuccess = false,
                Message = message
            };
        }
    }
}
