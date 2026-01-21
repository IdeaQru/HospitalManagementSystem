namespace HospitalManagementSystem.Domain.Exceptions
{
    public class AppointmentNotFoundException : EntityNotFoundException
    {
        public AppointmentNotFoundException(int appointmentId)
            : base("Appointment", appointmentId)
        {
        }

        public AppointmentNotFoundException(int appointmentId, string message)
            : base("Appointment", appointmentId, message)
        {
        }
    }
}
