namespace HospitalManagementSystem.Domain.ValueObjects
{
    public class TimeSlot
    {
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsBooked { get; set; }

        public TimeSlot()
        {
            IsBooked = false;
        }

        public TimeSlot(DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            Date = date;
            StartTime = startTime;
            EndTime = endTime;
            IsBooked = false;
        }

        public bool ContainsTime(DateTime dateTime)
        {
            return Date.Date == dateTime.Date && StartTime <= dateTime.TimeOfDay && EndTime >= dateTime.TimeOfDay;
        }

        public override string ToString()
        {
            return $"{Date:yyyy-MM-dd} {StartTime:hh\\:mm} - {EndTime:hh\\:mm}";
        }
    }
}
