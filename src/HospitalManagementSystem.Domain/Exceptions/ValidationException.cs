namespace HospitalManagementSystem.Domain.Exceptions
{
    // Exception untuk validation errors
    public class ValidationException : DomainException
    {
        public Dictionary<string, string[]> Errors { get; }

        public ValidationException()
            : base("One or more validation errors occurred.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(Dictionary<string, string[]> errors)
            : this()
        {
            Errors = errors;
        }

        public ValidationException(string propertyName, string errorMessage)
            : this()
        {
            Errors.Add(propertyName, new[] { errorMessage });
        }
    }
}
