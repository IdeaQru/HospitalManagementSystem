using HospitalManagementSystem.Domain.Entities;

namespace HospitalManagementSystem.Domain.Repositories
{
    public interface IPatientRepository : IRepository<Patient>
    {
        IEnumerable<Patient> GetPatientsByBloodType(string bloodType);
        Patient GetByMedicalRecordNumber(string mrn);
        IEnumerable<Patient> GetPatientsWithAllergy(string allergen);
        IEnumerable<Patient> GetPatientsWithUpcomingAppointments();
    }
}
