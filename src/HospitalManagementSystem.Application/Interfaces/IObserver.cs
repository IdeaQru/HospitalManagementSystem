using HospitalManagementSystem.Application.DTOs;

namespace HospitalManagementSystem.Application.Interfaces
{
    public interface IObserver
    {
        void Update(ISubject subject, NotificationData data);
    }
}
