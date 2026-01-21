namespace HospitalManagementSystem.Domain.Entities
{
    public class MedicalRecord
    {
        public int Id { get; set; }
        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }
        public DateTime Date { get; set; }
        public string Diagnosis { get; set; }
        public string Prescription { get; set; }
        public string LabResults { get; set; }
        public string Notes { get; set; }
        public int? AppointmentId { get; set; } // Link to appointment if applicable

        public MedicalRecord()
        {
            Date = DateTime.Now;
        }

        // Validation
        public bool IsValid()
        {
            return Patient != null &&
                   Doctor != null &&
                   !string.IsNullOrWhiteSpace(Diagnosis);
        }

        public override string ToString()
        {
            return $"Medical Record #{Id} - {Patient?.Name} by Dr. {Doctor?.Name} on {Date:yyyy-MM-dd}";
        }
    }
}
