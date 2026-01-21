using HospitalManagementSystem.Domain.ValueObjects;

namespace HospitalManagementSystem.Domain.Entities
{
    public class Doctor : Person
    {
        public string Specialization { get; set; }
        public string LicenseNumber { get; set; }
        public int YearsOfExperience { get; set; }
        public List<TimeSlot> AvailableTimeSlots { get; set; }
        public List<Appointment> Appointments { get; set; }

        public Doctor()
        {
            AvailableTimeSlots = new List<TimeSlot>();
            Appointments = new List<Appointment>();
        }

        public override string GetRole()
        {
            return "Doctor";
        }

        public override string GetFullInfo()
        {
            return $"Dr. {Name} ({Specialization}) - {YearsOfExperience} years experience";
        }

        public bool IsAvailable(DateTime dateTime)
        {
            if (AvailableTimeSlots == null || !AvailableTimeSlots.Any())
                return false;

            return AvailableTimeSlots.Any(slot => slot.Date.Date == dateTime.Date && slot.StartTime <= dateTime.TimeOfDay && slot.EndTime >= dateTime.TimeOfDay && !slot.IsBooked);
        }

        public List<TimeSlot> GetAvailableSlots(DateTime date)
        {
            return AvailableTimeSlots?.Where(slot => slot.Date.Date == date.Date && !slot.IsBooked).OrderBy(slot => slot.StartTime).ToList() ?? new List<TimeSlot>();
        }

        public bool BookTimeSlot(DateTime dateTime)
        {
            var slot = AvailableTimeSlots?.FirstOrDefault(s => s.ContainsTime(dateTime) && !s.IsBooked);

            if (slot != null)
            {
                slot.IsBooked = true;
                return true;
            }

            return false;
        }

        public override bool IsValid()
        {
            return base.IsValid() && !string.IsNullOrWhiteSpace(Specialization) && !string.IsNullOrWhiteSpace(LicenseNumber) && YearsOfExperience >= 0;
        }
    }
}
