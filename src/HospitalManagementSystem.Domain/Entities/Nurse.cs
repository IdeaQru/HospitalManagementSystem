namespace HospitalManagementSystem.Domain.Entities
{
    public class Nurse : Person
    {
        public string Department { get; set; }
        public string ShiftSchedule { get; set; }

        public override string GetRole()
        {
            return "Nurse";
        }

        public override string GetFullInfo()
        {
            return $"{base.GetFullInfo()} - Department: {Department}";
        }

        public override bool IsValid()
        {
            return base.IsValid() && !string.IsNullOrWhiteSpace(Department);
        }
    }
}
