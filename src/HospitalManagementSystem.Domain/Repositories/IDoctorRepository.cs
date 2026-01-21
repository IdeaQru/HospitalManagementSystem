using HospitalManagementSystem.Domain.Entities;

namespace HospitalManagementSystem.Domain.Repositories
{
    public interface IDoctorRepository : IRepository<Doctor>
    {
        // Custom query methods
        IEnumerable<Doctor> GetDoctorsBySpecialization(string specialization);
        Doctor GetByLicenseNumber(string licenseNumber);
        IEnumerable<Doctor> GetAvailableDoctors(DateTime date);
        IEnumerable<Doctor> GetDoctorsByExperience(int minYears);
    }
}
