namespace HospitalManagementSystem.Infrastructure.Configuration
{
    // Thread-safe Singleton pattern
    public sealed class HospitalConfiguration
    {
        // Lazy initialization untuk thread safety
        private static readonly Lazy<HospitalConfiguration> _instance = 
            new Lazy<HospitalConfiguration>(() => new HospitalConfiguration());

        // Private constructor untuk prevent instantiation dari luar
        private HospitalConfiguration()
        {
            LoadConfiguration();
        }

        // Public accessor untuk singleton instance
        public static HospitalConfiguration Instance => _instance.Value;

        // Configuration properties
        public string HospitalName { get; private set; }
        public string Address { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Email { get; private set; }
        public Dictionary<DayOfWeek, TimeRange> WorkingHours { get; private set; }
        public int AppointmentDurationMinutes { get; private set; }
        public int MaxAppointmentsPerDoctorPerDay { get; private set; }
        public string Currency { get; private set; }

        private void LoadConfiguration()
        {
            // Load configuration (hard-coded untuk demo)
            // Dalam real-world, ini akan load dari appsettings.json atau database
            HospitalName = "RS Harapan Sehat";
            Address = "Jl. Kesehatan No. 123, Surabaya, Jawa Timur 60111";
            PhoneNumber = "+62 31 1234 5678";
            Email = "info@rsharapansehat.com";
            AppointmentDurationMinutes = 30;
            MaxAppointmentsPerDoctorPerDay = 20;
            Currency = "IDR";

            // Setup working hours (Senin - Sabtu)
            WorkingHours = new Dictionary<DayOfWeek, TimeRange>
            {
                { DayOfWeek.Monday, new TimeRange(new TimeSpan(8, 0, 0), new TimeSpan(17, 0, 0)) },
                { DayOfWeek.Tuesday, new TimeRange(new TimeSpan(8, 0, 0), new TimeSpan(17, 0, 0)) },
                { DayOfWeek.Wednesday, new TimeRange(new TimeSpan(8, 0, 0), new TimeSpan(17, 0, 0)) },
                { DayOfWeek.Thursday, new TimeRange(new TimeSpan(8, 0, 0), new TimeSpan(17, 0, 0)) },
                { DayOfWeek.Friday, new TimeRange(new TimeSpan(8, 0, 0), new TimeSpan(17, 0, 0)) },
                { DayOfWeek.Saturday, new TimeRange(new TimeSpan(8, 0, 0), new TimeSpan(13, 0, 0)) }
                // Sunday tidak ada (rumah sakit tutup)
            };
        }

        public bool IsWorkingDay(DayOfWeek day)
        {
            return WorkingHours.ContainsKey(day);
        }

        public bool IsWithinWorkingHours(DateTime dateTime)
        {
            if (!IsWorkingDay(dateTime.DayOfWeek))
                return false;

            var workingHours = WorkingHours[dateTime.DayOfWeek];
            return dateTime.TimeOfDay >= workingHours.StartTime && 
                   dateTime.TimeOfDay <= workingHours.EndTime;
        }

        public void DisplayConfiguration()
        {
            Console.WriteLine("\n=== HOSPITAL CONFIGURATION ===");
            Console.WriteLine($"Name: {HospitalName}");
            Console.WriteLine($"Address: {Address}");
            Console.WriteLine($"Phone: {PhoneNumber}");
            Console.WriteLine($"Email: {Email}");
            Console.WriteLine($"Appointment Duration: {AppointmentDurationMinutes} minutes");
            Console.WriteLine($"Max Appointments/Doctor/Day: {MaxAppointmentsPerDoctorPerDay}");
            Console.WriteLine("\nWorking Hours:");
            foreach (var wh in WorkingHours)
            {
                Console.WriteLine($"  {wh.Key}: {wh.Value.StartTime:hh\\:mm} - {wh.Value.EndTime:hh\\:mm}");
            }
            Console.WriteLine("===============================\n");
        }
    }

    // Helper class untuk time range
    public class TimeRange
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public TimeRange(TimeSpan startTime, TimeSpan endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        public override string ToString()
        {
            return $"{StartTime:hh\\:mm} - {EndTime:hh\\:mm}";
        }
    }
}
