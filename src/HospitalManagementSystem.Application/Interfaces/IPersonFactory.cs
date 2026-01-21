using HospitalManagementSystem.Domain.Entities;
using HospitalManagementSystem.Domain.Enums;
using HospitalManagementSystem.Domain.ValueObjects;

namespace HospitalManagementSystem.Application.Interfaces
{
    public interface IPersonFactory
    {
        Person CreatePerson(PersonType type, PersonData data);
    }
}
