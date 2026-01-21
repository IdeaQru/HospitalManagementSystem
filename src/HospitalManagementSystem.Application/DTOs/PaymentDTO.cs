using HospitalManagementSystem.Domain.Enums;

namespace HospitalManagementSystem.Application.DTOs
{
    public class PaymentDTO
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string TransactionId { get; set; }
    }
}
