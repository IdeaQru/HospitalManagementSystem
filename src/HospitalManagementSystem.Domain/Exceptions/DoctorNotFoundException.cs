namespace HospitalManagementSystem.Domain.Exceptions
{
    public class DoctorNotFoundException : EntityNotFoundException
    {
        public DoctorNotFoundException(int doctorId)
            : base("Doctor", doctorId)
        {
        }

        public DoctorNotFoundException(string licenseNumber)
            : base("Doctor", licenseNumber, 
                   $"Doctor with License Number '{licenseNumber}' was not found.")
        {
        }
    }
}
