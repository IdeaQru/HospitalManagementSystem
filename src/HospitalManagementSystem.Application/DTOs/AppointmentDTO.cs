using HospitalManagementSystem.Domain.Enums;

namespace HospitalManagementSystem.Application.DTOs
{
    public class AppointmentDTO
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public int Duration { get; set; }
        public AppointmentStatus Status { get; set; }
        public string Reason { get; set; }
        public string Notes { get; set; }
    }
}
