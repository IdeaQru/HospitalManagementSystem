namespace HospitalManagementSystem.Domain.Entities
{
    public abstract class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string EmailAddress { get; set; }

        public abstract string GetRole();

        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - DateOfBirth.Year;
                if (DateOfBirth.Date > today.AddYears(-age)) age--;
                return age;
            }
        }
        public virtual string GetFullInfo()
        {
            return $"{Name} - {GetRole()} - {Age} years old";
        }

        public virtual bool IsValid()
        {
            return !string .IsNullOrWhiteSpace(Name) &&
                !string.IsNullOrWhiteSpace(Email) &&
                DateOfBirth < DateTime.Today;
        }


    }
}