using HospitalManagementSystem.Domain.Entities;
using HospitalManagementSystem.Domain.Enums;

namespace HospitalManagementSystem.Domain.Repositories
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        // Custom query methods
        IEnumerable<Payment> GetPaymentsByStatus(PaymentStatus status);
        IEnumerable<Payment> GetPendingPayments();
        IEnumerable<Payment> GetPaymentsByPaymentMethod(PaymentMethod method);
        Payment GetPaymentByAppointment(int appointmentId);
        IEnumerable<Payment> GetPaymentsByDateRange(DateTime startDate, DateTime endDate);
        decimal GetTotalRevenue(DateTime startDate, DateTime endDate);
    }
}
