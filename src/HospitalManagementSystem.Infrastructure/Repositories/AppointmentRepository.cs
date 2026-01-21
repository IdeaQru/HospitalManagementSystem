using HospitalManagementSystem.Domain.Entities;
using HospitalManagementSystem.Domain.Enums;
using HospitalManagementSystem.Domain.Repositories;

namespace HospitalManagementSystem.Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly Dictionary<int, Appointment> _appointments;
        private int _nextId;

        public AppointmentRepository()
        {
            _appointments = new Dictionary<int, Appointment>();
            _nextId = 1;
        }

        public Appointment GetById(int id)
        {
            return _appointments.ContainsKey(id) ? _appointments[id] : null;
        }

        public IEnumerable<Appointment> GetAll()
        {
            return _appointments.Values;
        }

        public void Add(Appointment entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.Id = _nextId++;
            _appointments[entity.Id] = entity;
        }

        public void Update(Appointment entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (_appointments.ContainsKey(entity.Id))
            {
                _appointments[entity.Id] = entity;
            }
        }

        public void Delete(int id)
        {
            if (_appointments.ContainsKey(id))
            {
                _appointments.Remove(id);
            }
        }

        // Custom query methods
        public IEnumerable<Appointment> GetAppointmentsByDate(DateTime date)
        {
            return _appointments.Values
                .Where(a => a.AppointmentDateTime.Date == date.Date)
                .OrderBy(a => a.AppointmentDateTime);
        }

        public IEnumerable<Appointment> GetAppointmentsByPatient(int patientId)
        {
            return _appointments.Values
                .Where(a => a.Patient != null && a.Patient.Id == patientId)
                .OrderByDescending(a => a.AppointmentDateTime);
        }

        public IEnumerable<Appointment> GetAppointmentsByDoctor(int doctorId)
        {
            return _appointments.Values
                .Where(a => a.Doctor != null && a.Doctor.Id == doctorId)
                .OrderByDescending(a => a.AppointmentDateTime);
        }

        public IEnumerable<Appointment> GetAppointmentsByStatus(AppointmentStatus status)
        {
            return _appointments.Values
                .Where(a => a.Status == status)
                .OrderBy(a => a.AppointmentDateTime);
        }

        public IEnumerable<Appointment> GetUpcomingAppointments()
        {
            return _appointments.Values
                .Where(a => a.AppointmentDateTime > DateTime.Now && 
                           a.Status == AppointmentStatus.Scheduled)
                .OrderBy(a => a.AppointmentDateTime);
        }

        public IEnumerable<Appointment> GetAppointmentsInDateRange(DateTime startDate, DateTime endDate)
        {
            return _appointments.Values
                .Where(a => a.AppointmentDateTime >= startDate && 
                           a.AppointmentDateTime <= endDate)
                .OrderBy(a => a.AppointmentDateTime);
        }

        public bool HasConflictingAppointment(int doctorId, DateTime appointmentDateTime, int duration)
        {
            var endDateTime = appointmentDateTime.AddMinutes(duration);

            return _appointments.Values.Any(a =>
                a.Doctor != null &&
                a.Doctor.Id == doctorId &&
                a.Status == AppointmentStatus.Scheduled &&
                (
                    (appointmentDateTime >= a.AppointmentDateTime && appointmentDateTime < a.EndDateTime) ||
                    (endDateTime > a.AppointmentDateTime && endDateTime <= a.EndDateTime) ||
                    (appointmentDateTime <= a.AppointmentDateTime && endDateTime >= a.EndDateTime)
                )
            );
        }
    }
}
