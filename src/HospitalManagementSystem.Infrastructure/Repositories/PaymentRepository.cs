using HospitalManagementSystem.Domain.Entities;
using HospitalManagementSystem.Domain.Enums;
using HospitalManagementSystem.Domain.Repositories;

namespace HospitalManagementSystem.Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly Dictionary<int, Payment> _payments;
        private int _nextId;

        public PaymentRepository()
        {
            _payments = new Dictionary<int, Payment>();
            _nextId = 1;
        }

        public Payment GetById(int id)
        {
            return _payments.ContainsKey(id) ? _payments[id] : null;
        }

        public IEnumerable<Payment> GetAll()
        {
            return _payments.Values;
        }

        public void Add(Payment entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.Id = _nextId++;
            _payments[entity.Id] = entity;
        }

        public void Update(Payment entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (_payments.ContainsKey(entity.Id))
            {
                _payments[entity.Id] = entity;
            }
        }

        public void Delete(int id)
        {
            if (_payments.ContainsKey(id))
            {
                _payments.Remove(id);
            }
        }

        // Custom query methods
        public IEnumerable<Payment> GetPaymentsByStatus(PaymentStatus status)
        {
            return _payments.Values
                .Where(p => p.PaymentStatus == status)
                .OrderByDescending(p => p.CreatedAt);
        }

        public IEnumerable<Payment> GetPendingPayments()
        {
            return _payments.Values
                .Where(p => p.PaymentStatus == PaymentStatus.Pending)
                .OrderBy(p => p.CreatedAt);
        }

        public IEnumerable<Payment> GetPaymentsByPaymentMethod(PaymentMethod method)
        {
            return _payments.Values
                .Where(p => p.PaymentMethod == method)
                .OrderByDescending(p => p.CreatedAt);
        }

        public Payment GetPaymentByAppointment(int appointmentId)
        {
            return _payments.Values
                .FirstOrDefault(p => p.Appointment != null && p.Appointment.Id == appointmentId);
        }

        public IEnumerable<Payment> GetPaymentsByDateRange(DateTime startDate, DateTime endDate)
        {
            return _payments.Values
                .Where(p => p.PaymentDate.HasValue && 
                           p.PaymentDate.Value >= startDate && 
                           p.PaymentDate.Value <= endDate)
                .OrderByDescending(p => p.PaymentDate);
        }

        public decimal GetTotalRevenue(DateTime startDate, DateTime endDate)
        {
            return _payments.Values
                .Where(p => p.PaymentStatus == PaymentStatus.Completed &&
                           p.PaymentDate.HasValue &&
                           p.PaymentDate.Value >= startDate &&
                           p.PaymentDate.Value <= endDate)
                .Sum(p => p.Amount);
        }
    }
}
