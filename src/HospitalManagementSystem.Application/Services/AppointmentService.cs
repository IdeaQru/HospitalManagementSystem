using HospitalManagementSystem.Domain.Entities;
using HospitalManagementSystem.Domain.Enums;
using HospitalManagementSystem.Domain.Repositories;
using HospitalManagementSystem.Domain.Exceptions;
using HospitalManagementSystem.Application.Results;
using HospitalManagementSystem.Application.Interfaces;
using HospitalManagementSystem.Application.DTOs;

namespace HospitalManagementSystem.Application.Services
{
    public class AppointmentService : ISubject
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly List<IObserver> _observers;
        private readonly int _defaultAppointmentDuration;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository,
            int defaultAppointmentDuration = 30)
        {
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _observers = new List<IObserver>();
            _defaultAppointmentDuration = defaultAppointmentDuration;
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

        // Business Logic: Book Appointment
        public AppointmentResult BookAppointment(int patientId, int doctorId, DateTime appointmentDateTime, string reason, int? duration = null)
        {
            try
            {
                // 1. Validate patient exists
                var patient = _patientRepository.GetById(patientId);
                if (patient == null)
                    throw new PatientNotFoundException(patientId);

                // 2. Validate doctor exists
                var doctor = _doctorRepository.GetById(doctorId);
                if (doctor == null)
                    throw new DoctorNotFoundException(doctorId);

                // 3. Validate appointment is in the future
                if (appointmentDateTime <= DateTime.Now)
                    throw InvalidAppointmentException.AppointmentInPast();

                // 4. Check doctor availability
                if (!doctor.IsAvailable(appointmentDateTime))
                    throw InvalidAppointmentException.DoctorNotAvailable(appointmentDateTime);

                // 5. Check for conflicting appointments
                int appointmentDuration = duration ?? _defaultAppointmentDuration;
                bool hasConflict = _appointmentRepository.HasConflictingAppointment(
                    doctorId, appointmentDateTime, appointmentDuration);

                if (hasConflict)
                    throw InvalidAppointmentException.ConflictingAppointment();

                // 6. Create appointment
                var appointment = new Appointment
                {
                    Patient = patient,
                    Doctor = doctor,
                    AppointmentDateTime = appointmentDateTime,
                    Duration = appointmentDuration,
                    Reason = reason,
                    Status = AppointmentStatus.Scheduled
                };

                // 7. Validate appointment
                if (!appointment.IsValid())
                    return AppointmentResult.Failure("Invalid appointment data");

                // 8. Save appointment
                _appointmentRepository.Add(appointment);

                // 9. Book doctor's time slot
                doctor.BookTimeSlot(appointmentDateTime);
                _doctorRepository.Update(doctor);

                // 10. Send notifications
                SendAppointmentNotification(appointment, "Appointment Booked");

                return AppointmentResult.Success(appointment, 
                    $"Appointment successfully booked for {appointmentDateTime:yyyy-MM-dd HH:mm}");
            }
            catch (DomainException ex)
            {
                return AppointmentResult.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                return AppointmentResult.Failure($"An error occurred: {ex.Message}");
            }
        }

        // Business Logic: Cancel Appointment
        public AppointmentResult CancelAppointment(int appointmentId)
        {
            try
            {
                var appointment = _appointmentRepository.GetById(appointmentId);
                if (appointment == null)
                    throw new AppointmentNotFoundException(appointmentId);

                if (!appointment.CanBeCancelled())
                    throw InvalidAppointmentException.CannotBeCancelled();

                appointment.Cancel();
                _appointmentRepository.Update(appointment);

                SendAppointmentNotification(appointment, "Appointment Cancelled");

                return AppointmentResult.Success(appointment, "Appointment cancelled successfully");
            }
            catch (DomainException ex)
            {
                return AppointmentResult.Failure(ex.Message);
            }
        }

        // Business Logic: Complete Appointment
        public AppointmentResult CompleteAppointment(int appointmentId)
        {
            try
            {
                var appointment = _appointmentRepository.GetById(appointmentId);
                if (appointment == null)
                    throw new AppointmentNotFoundException(appointmentId);

                appointment.Complete();
                _appointmentRepository.Update(appointment);

                SendAppointmentNotification(appointment, "Appointment Completed");

                return AppointmentResult.Success(appointment, "Appointment marked as completed");
            }
            catch (DomainException ex)
            {
                return AppointmentResult.Failure(ex.Message);
            }
        }

        // Business Logic: Get Available Slots
        public List<DateTime> GetAvailableSlots(int doctorId, DateTime date)
        {
            var doctor = _doctorRepository.GetById(doctorId);
            if (doctor == null)
                throw new DoctorNotFoundException(doctorId);

            var availableSlots = doctor.GetAvailableSlots(date);

            var dateTimeSlots = new List<DateTime>();
            foreach (var slot in availableSlots)
            {
                var slotDateTime = slot.Date.Date.Add(slot.StartTime);
                dateTimeSlots.Add(slotDateTime);
            }

            return dateTimeSlots;
        }

        // Business Logic: Get Upcoming Appointments
        public List<Appointment> GetUpcomingAppointments()
        {
            return _appointmentRepository.GetUpcomingAppointments().ToList();
        }

        // Business Logic: Get Appointments by Patient
        public List<Appointment> GetAppointmentsByPatient(int patientId)
        {
            return _appointmentRepository.GetAppointmentsByPatient(patientId).ToList();
        }

        // Business Logic: Get Appointments by Doctor
        public List<Appointment> GetAppointmentsByDoctor(int doctorId)
        {
            return _appointmentRepository.GetAppointmentsByDoctor(doctorId).ToList();
        }

        // Helper: Send Notification
        private void SendAppointmentNotification(Appointment appointment, string subject)
        {
            var notificationData = new NotificationData(
                appointment.Patient.Name,
                appointment.Patient.Email,
                appointment.Patient.PhoneNumber,
                subject,
                $"Appointment with Dr. {appointment.Doctor.Name} on {appointment.AppointmentDateTime:yyyy-MM-dd HH:mm}",
                "Appointment"
            );

            Notify(notificationData);
        }
    }
}
