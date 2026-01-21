using HospitalManagementSystem.Domain.Entities;
using HospitalManagementSystem.Domain.Enums;

namespace HospitalManagementSystem.Domain.Repositories
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        // Custom query methods
        IEnumerable<Appointment> GetAppointmentsByDate(DateTime date);
        IEnumerable<Appointment> GetAppointmentsByPatient(int patientId);
        IEnumerable<Appointment> GetAppointmentsByDoctor(int doctorId);
        IEnumerable<Appointment> GetAppointmentsByStatus(AppointmentStatus status);
        IEnumerable<Appointment> GetUpcomingAppointments();
        IEnumerable<Appointment> GetAppointmentsInDateRange(DateTime startDate, DateTime endDate);
        bool HasConflictingAppointment(int doctorId, DateTime appointmentDateTime, int duration);
    }
}
