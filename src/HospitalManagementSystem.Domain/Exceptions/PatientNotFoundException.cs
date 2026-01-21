namespace HospitalManagementSystem.Domain.Exceptions
{
    public class PatientNotFoundException : EntityNotFoundException
    {
        public PatientNotFoundException(int patientId)
            : base("Patient", patientId)
        {
        }

        public PatientNotFoundException(string medicalRecordNumber)
            : base("Patient", medicalRecordNumber, 
                   $"Patient with Medical Record Number '{medicalRecordNumber}' was not found.")
        {
        }
    }
}
