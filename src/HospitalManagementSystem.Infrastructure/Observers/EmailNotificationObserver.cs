using HospitalManagementSystem.Application.Interfaces;
using HospitalManagementSystem.Application.DTOs;

namespace HospitalManagementSystem.Infrastructure.Observers
{
    public class EmailNotificationObserver : IObserver
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;

        public EmailNotificationObserver(string smtpServer = "smtp.hospital.com", int smtpPort = 587)
        {
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
        }

        public void Update(ISubject subject, NotificationData data)
        {
            if (data == null || string.IsNullOrWhiteSpace(data.RecipientEmail))
                return;

            SendEmail(data);
        }

        private void SendEmail(NotificationData data)
        {
            // Simulate sending email
            // Dalam real-world, ini akan menggunakan SMTP client
            Console.WriteLine($"\n📧 EMAIL NOTIFICATION");
            Console.WriteLine($"   To: {data.RecipientEmail}");
            Console.WriteLine($"   Subject: {data.Subject}");
            Console.WriteLine($"   Message: {data.Message}");
            Console.WriteLine($"   Sent via: {_smtpServer}:{_smtpPort}");
            Console.WriteLine($"   Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            
            // Log untuk tracking
            LogEmailSent(data);
        }

        private void LogEmailSent(NotificationData data)
        {
            // Simulate logging
            Console.WriteLine($"   [LOG] Email notification sent to {data.RecipientEmail}");
        }
    }
}
