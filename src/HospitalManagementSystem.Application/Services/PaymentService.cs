using HospitalManagementSystem.Domain.Entities;
using HospitalManagementSystem.Domain.Enums;
using HospitalManagementSystem.Domain.Repositories;
using HospitalManagementSystem.Domain.Exceptions;
using HospitalManagementSystem.Domain.ValueObjects;
using HospitalManagementSystem.Application.Interfaces;
using HospitalManagementSystem.Application.Results;
using HospitalManagementSystem.Application.DTOs;

namespace HospitalManagementSystem.Application.Services
{
    public class PaymentService : ISubject
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private IPaymentStrategy _paymentStrategy;
        private readonly List<IObserver> _observers;

        public PaymentService(
            IPaymentRepository paymentRepository,
            IAppointmentRepository appointmentRepository)
        {
            _paymentRepository = paymentRepository;
            _appointmentRepository = appointmentRepository;
            _observers = new List<IObserver>();
        }

        // Strategy Pattern: Set payment strategy at runtime
        public void SetPaymentStrategy(IPaymentStrategy strategy)
        {
            _paymentStrategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
        }

        // Observer Pattern methods
        public void Attach(IObserver observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }

        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify(NotificationData data)
        {
            foreach (var observer in _observers)
            {
                observer.Update(this, data);
            }
        }

        // Business Logic: Process Payment
        public PaymentResult ProcessPayment(int appointmentId, decimal amount, PaymentMethod paymentMethod, PaymentDetails paymentDetails)
        {
            try
            {
                // 1. Validate appointment exists
                var appointment = _appointmentRepository.GetById(appointmentId);
                if (appointment == null)
                    throw new AppointmentNotFoundException(appointmentId);

                // 2. Check if payment already exists
                var existingPayment = _paymentRepository.GetPaymentByAppointment(appointmentId);
                if (existingPayment != null && existingPayment.PaymentStatus == PaymentStatus.Completed)
                {
                    return PaymentResult.Failure("Payment already completed for this appointment");
                }

                // 3. Validate amount
                if (amount <= 0)
                    throw InvalidPaymentException.InvalidAmount();

                // 4. Ensure strategy is set
                if (_paymentStrategy == null)
                    return PaymentResult.Failure("Payment strategy not set");

                // 5. Process payment using strategy
                var paymentResult = _paymentStrategy.ProcessPayment(amount, paymentDetails);

                if (!paymentResult.IsSuccess)
                    return paymentResult;

                // 6. Create or update payment entity
                Payment payment;
                if (existingPayment != null)
                {
                    payment = existingPayment;
                    payment.MarkAsPaid(paymentResult.TransactionId);
                    _paymentRepository.Update(payment);
                }
                else
                {
                    payment = new Payment
                    {
                        Appointment = appointment,
                        Amount = amount,
                        PaymentMethod = paymentMethod,
                        TransactionId = paymentResult.TransactionId
                    };
                    payment.MarkAsPaid(paymentResult.TransactionId);
                    _paymentRepository.Add(payment);
                }

                // 7. Send notification
                SendPaymentNotification(payment, appointment);

                return PaymentResult.Success(
                    paymentResult.TransactionId,
                    $"Payment of Rp {amount:N0} processed successfully via {paymentMethod}"
                );
            }
            catch (DomainException ex)
            {
                return PaymentResult.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                return PaymentResult.Failure($"An error occurred: {ex.Message}");
            }
        }

        // Business Logic: Get Payment by Appointment
        public Payment GetPaymentByAppointment(int appointmentId)
        {
            return _paymentRepository.GetPaymentByAppointment(appointmentId);
        }

        // Business Logic: Get Pending Payments
        public List<Payment> GetPendingPayments()
        {
            return _paymentRepository.GetPendingPayments().ToList();
        }

        // Business Logic: Refund Payment
        public PaymentResult RefundPayment(int paymentId)
        {
            try
            {
                var payment = _paymentRepository.GetById(paymentId);
                if (payment == null)
                    return PaymentResult.Failure("Payment not found");

                if (!payment.Refund())
                    return PaymentResult.Failure("Payment cannot be refunded");

                _paymentRepository.Update(payment);

                var notificationData = new NotificationData(
                    payment.Appointment.Patient.Name,
                    payment.Appointment.Patient.Email,
                    payment.Appointment.Patient.PhoneNumber,
                    "Payment Refunded",
                    $"Your payment of Rp {payment.Amount:N0} has been refunded",
                    "Payment"
                );
                Notify(notificationData);

                return PaymentResult.Success(payment.TransactionId, "Payment refunded successfully");
            }
            catch (Exception ex)
            {
                return PaymentResult.Failure($"Refund failed: {ex.Message}");
            }
        }

        // Business Logic: Get Total Revenue
        public decimal GetTotalRevenue(DateTime startDate, DateTime endDate)
        {
            return _paymentRepository.GetTotalRevenue(startDate, endDate);
        }

        // Helper: Send Payment Notification
        private void SendPaymentNotification(Payment payment, Appointment appointment)
        {
            var notificationData = new NotificationData(
                appointment.Patient.Name,
                appointment.Patient.Email,
                appointment.Patient.PhoneNumber,
                "Payment Confirmation",
                $"Payment of Rp {payment.Amount:N0} received. Transaction ID: {payment.TransactionId}",
                "Payment"
            );

            Notify(notificationData);
        }
    }
}
