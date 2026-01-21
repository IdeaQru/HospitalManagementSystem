namespace HospitalManagementSystem.Application.DTOs
{
    public class NotificationData
    {
        public string RecipientName { get; set; }
        public string RecipientEmail { get; set; }
        public string RecipientPhone { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public string NotificationType { get; set; } // "Appointment", "Payment", "Reminder", etc.

        public NotificationData()
        {
            CreatedAt = DateTime.Now;
        }

        public NotificationData(string recipientName, string recipientEmail, string recipientPhone, string subject, string message, string notificationType = "General")
        {
            RecipientName = recipientName;
            RecipientEmail = recipientEmail;
            RecipientPhone = recipientPhone;
            Subject = subject;
            Message = message;
            NotificationType = notificationType;
            CreatedAt = DateTime.Now;
        }
    }
}
