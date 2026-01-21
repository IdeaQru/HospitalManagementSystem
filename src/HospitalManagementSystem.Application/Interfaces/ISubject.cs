using HospitalManagementSystem.Application.DTOs;

namespace HospitalManagementSystem.Application.Interfaces
{
    public interface ISubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Notify(NotificationData data);
    }
}
