using HospitalManagementSystem.Domain.Entities;

namespace HospitalManagementSystem.Application.Results
{
    public class AppointmentResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public Appointment Appointment { get; set; }
        public List<string> Errors { get; set; }

        public AppointmentResult()
        {
            Errors = new List<string>();
        }

        public static AppointmentResult Success(Appointment appointment, string message = "Appointment created successfully")
        {
            return new AppointmentResult
            {
                IsSuccess = true,
                Message = message,
                Appointment = appointment
            };
        }

        public static AppointmentResult Failure(string message)
        {
            return new AppointmentResult
            {
                IsSuccess = false,
                Message = message
            };
        }

        public static AppointmentResult Failure(List<string> errors)
        {
            return new AppointmentResult
            {
                IsSuccess = false,
                Message = "Appointment creation failed",
                Errors = errors
            };
        }
    }
}
