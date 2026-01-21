namespace HospitalManagementSystem.Domain.ValueObjects
{
  public class PersonData
    {
        public string Name { get; set;}
        public DateTime DateOfBirth { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string MedicalRecordNumber { get; set; }

        public string BloodType{ get; set; }

        public List<string> Allergies { get; set; }

        public string Specialization { get; set; }

        public string LicenseNumber { get; set; }

        public int YearsOfExperience { get; set; }

        public string Department { get; set; }
        public string ShiftSchedule { get; set; }

        public PersonData()
        {
            Allergies = new List<string>();
        }

    }
}