using HospitalManagementSystem.Application.Interfaces;
using HospitalManagementSystem.Application.DTOs;

namespace HospitalManagementSystem.Infrastructure.Observers
{
    public class SMSNotificationObserver : IObserver
    {
        private readonly string _smsGateway;

        public SMSNotificationObserver(string smsGateway = "SMS Gateway API")
        {
            _smsGateway = smsGateway;
        }

        public void Update(ISubject subject, NotificationData data)
        {
            if (data == null || string.IsNullOrWhiteSpace(data.RecipientPhone))
                return;

            SendSMS(data);
        }

        private void SendSMS(NotificationData data)
        {
            // Simulate sending SMS
            // Dalam real-world, ini akan call ke SMS gateway API
            Console.WriteLine($"\n📱 SMS NOTIFICATION");
            Console.WriteLine($"   To: {data.RecipientPhone}");
            Console.WriteLine($"   Message: {data.Message}");
            Console.WriteLine($"   Sent via: {_smsGateway}");
            Console.WriteLine($"   Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            
            // Truncate message jika terlalu panjang (160 karakter limit)
            if (data.Message.Length > 160)
            {
                Console.WriteLine($"   [WARNING] Message truncated to 160 characters");
            }
            
            LogSMSSent(data);
        }

        private void LogSMSSent(NotificationData data)
        {
            // Simulate logging
            Console.WriteLine($"   [LOG] SMS notification sent to {data.RecipientPhone}");
        }
    }
}
