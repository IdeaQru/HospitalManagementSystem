using HospitalManagementSystem.Domain.Enums;

namespace HospitalManagementSystem.Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public Appointment Appointment { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string TransactionId { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }

        public Payment()
        {
            PaymentStatus = PaymentStatus.Pending;
            CreatedAt = DateTime.Now;
        }

        // Business logic: Process payment
        public bool MarkAsPaid(string transactionId)
        {
            if (PaymentStatus != PaymentStatus.Pending)
                return false;

            PaymentStatus = PaymentStatus.Completed;
            PaymentDate = DateTime.Now;
            TransactionId = transactionId;
            return true;
        }

        // Business logic: Mark as failed
        public bool MarkAsFailed(string reason)
        {
            if (PaymentStatus == PaymentStatus.Completed)
                return false;

            PaymentStatus = PaymentStatus.Failed;
            Notes = reason;
            return true;
        }

        // Business logic: Refund
        public bool Refund()
        {
            if (PaymentStatus != PaymentStatus.Completed)
                return false;

            PaymentStatus = PaymentStatus.Refunded;
            return true;
        }

        // Validation
        public bool IsValid()
        {
            return Appointment != null && Amount > 0;
        }

        public override string ToString()
        {
            return $"Payment #{Id} - Rp {Amount:N0} ({PaymentStatus}) for Appointment #{Appointment?.Id}";
        }
    }
}
