using HospitalManagementSystem.Application.Interfaces;
using HospitalManagementSystem.Application.DTOs;

namespace HospitalManagementSystem.Infrastructure.Observers
{
    public class WhatsAppNotificationObserver : IObserver
    {
        private readonly string _whatsappApiUrl;

        public WhatsAppNotificationObserver(string whatsappApiUrl = "https://api.whatsapp.com/send")
        {
            _whatsappApiUrl = whatsappApiUrl;
        }

        public void Update(ISubject subject, NotificationData data)
        {
            if (data == null || string.IsNullOrWhiteSpace(data.RecipientPhone))
                return;

            SendWhatsApp(data);
        }

        private void SendWhatsApp(NotificationData data)
        {
            // Simulate sending WhatsApp message
            // Dalam real-world, ini akan call ke WhatsApp Business API
            Console.WriteLine($"\n💬 WHATSAPP NOTIFICATION");
            Console.WriteLine($"   To: {data.RecipientPhone}");
            Console.WriteLine($"   Message: {data.Message}");
            Console.WriteLine($"   API: {_whatsappApiUrl}");
            Console.WriteLine($"   Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            
            // Format WhatsApp message
            string formattedMessage = FormatWhatsAppMessage(data);
            Console.WriteLine($"   Formatted: {formattedMessage}");
            
            LogWhatsAppSent(data);
        }

        private string FormatWhatsAppMessage(NotificationData data)
        {
            // Format message untuk WhatsApp dengan emoji dan formatting
            return $"🏥 *{data.Subject}*\n\n{data.Message}\n\n_Hospital Management System_";
        }

        private void LogWhatsAppSent(NotificationData data)
        {
            // Simulate logging
            Console.WriteLine($"   [LOG] WhatsApp notification sent to {data.RecipientPhone}");
        }
    }
}
