namespace HospitalManagementSystem.Domain.Exceptions
{
    // Base exception untuk semua domain-related errors
    public class DomainException : Exception
    {
        public DomainException()
        {
        }

        public DomainException(string message) : base(message)
        {
        }

        public DomainException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
