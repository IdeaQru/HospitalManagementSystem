namespace HospitalManagementSystem.Domain.Exceptions
{
    // Exception untuk appointment-related business rule violations
    public class InvalidAppointmentException : DomainException
    {
        public InvalidAppointmentException(string message) : base(message)
        {
        }

        public static InvalidAppointmentException DoctorNotAvailable(DateTime appointmentDateTime)
        {
            return new InvalidAppointmentException(
                $"Doctor is not available at {appointmentDateTime:yyyy-MM-dd HH:mm}");
        }

        public static InvalidAppointmentException AppointmentInPast()
        {
            return new InvalidAppointmentException(
                "Cannot create appointment in the past");
        }

        public static InvalidAppointmentException ConflictingAppointment()
        {
            return new InvalidAppointmentException(
                "There is a conflicting appointment at this time");
        }

        public static InvalidAppointmentException CannotBeCancelled()
        {
            return new InvalidAppointmentException(
                "Appointment cannot be cancelled");
        }
    }
}
