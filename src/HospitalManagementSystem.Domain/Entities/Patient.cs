namespace HospitalManagementSystem.Domain.Entities
{
    public class Patient : Person
    {
        public string MedicalRecordNumber { get; set; }
        public string BloodType { get; set; }
        public List<string> Allergies { get; set; }
        public List<Appointment> Appointments { get; set; }
        public List<MedicalRecord> MedicalRecords { get; set; }

        public Patient()
        {
            Allergies = new List<string>();
            Appointments = new List<Appointment>();
            MedicalRecords = new List<MedicalRecord>();
        }

        public override string GetRole()
        {
            return "Patient";
        }

        public override string GetFullInfo()
        {
            return $"{base.GetFullInfo()} - MRN: {MedicalRecordNumber}, Blood Type: {BloodType}";
        }

        public bool HasAllergy(string allergen)
        {
            return Allergies != null && Allergies.Any(a => a.Equals(allergen, StringComparison.OrdinalIgnoreCase));
        }

        public List<Appointment> GetUpcomingAppointments()
        {
            return Appointments?.Where(a => a.AppointmentDateTime > DateTime.Now && a.Status == Enums.AppointmentStatus.Scheduled).OrderBy(a => a.AppointmentDateTime).ToList() ?? new List<Appointment>();
        }

        public override bool IsValid()
        {
            return base.IsValid() && !string.IsNullOrWhiteSpace(MedicalRecordNumber) && !string.IsNullOrWhiteSpace(BloodType);
        }
    }
}
