using HospitalManagementSystem.Domain.Enums;

namespace HospitalManagementSystem.Domain.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public int Duration { get; set; }
        public AppointmentStatus Status { get; set; }
        public string Reason { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Appointment()
        {
            Status = AppointmentStatus.Scheduled;
            CreatedAt = DateTime.Now;
            Duration = 30;
        }

        public DateTime EndDateTime
        {
            get { return AppointmentDateTime.AddMinutes(Duration); }
        }

        public bool IsPast()
        {
            return EndDateTime < DateTime.Now;
        }

        public bool CanBeCancelled()
        {
            return Status == AppointmentStatus.Scheduled && AppointmentDateTime > DateTime.Now;
        }

        public bool Cancel()
        {
            if (!CanBeCancelled())
                return false;

            Status = AppointmentStatus.Cancelled;
            UpdatedAt = DateTime.Now;
            return true;
        }

        public bool Complete()
        {
            if (Status != AppointmentStatus.Scheduled)
                return false;

            Status = AppointmentStatus.Completed;
            UpdatedAt = DateTime.Now;
            return true;
        }

        public bool MarkAsNoShow()
        {
            if (Status != AppointmentStatus.Scheduled || !IsPast())
                return false;

            Status = AppointmentStatus.NoShow;
            UpdatedAt = DateTime.Now;
            return true;
        }

        public bool IsValid()
        {
            return Patient != null && Doctor != null && AppointmentDateTime > DateTime.Now && Duration > 0 && !string.IsNullOrWhiteSpace(Reason);
        }

        public override string ToString()
        {
            return $"Appointment #{Id} - {Patient?.Name} with Dr. {Doctor?.Name} on {AppointmentDateTime:yyyy-MM-dd HH:mm}";
        }
    }
}
