using HospitalManagementSystem.Domain.Entities;
using HospitalManagementSystem.Domain.Repositories;

namespace HospitalManagementSystem.Infrastructure.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly Dictionary<int, Doctor> _doctors;
        private int _nextId;

        public DoctorRepository()
        {
            _doctors = new Dictionary<int, Doctor>();
            _nextId = 1;
        }

        public Doctor GetById(int id)
        {
            return _doctors.ContainsKey(id) ? _doctors[id] : null;
        }

        public IEnumerable<Doctor> GetAll()
        {
            return _doctors.Values;
        }

        public void Add(Doctor entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.Id = _nextId++;
            _doctors[entity.Id] = entity;
        }

        public void Update(Doctor entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (_doctors.ContainsKey(entity.Id))
            {
                _doctors[entity.Id] = entity;
            }
        }

        public void Delete(int id)
        {
            if (_doctors.ContainsKey(id))
            {
                _doctors.Remove(id);
            }
        }

        // Custom query methods
        public IEnumerable<Doctor> GetDoctorsBySpecialization(string specialization)
        {
            return _doctors.Values
                .Where(d => d.Specialization.Equals(specialization, StringComparison.OrdinalIgnoreCase));
        }

        public Doctor GetByLicenseNumber(string licenseNumber)
        {
            return _doctors.Values
                .FirstOrDefault(d => d.LicenseNumber == licenseNumber);
        }

        public IEnumerable<Doctor> GetAvailableDoctors(DateTime date)
        {
            return _doctors.Values
                .Where(d => d.GetAvailableSlots(date).Any());
        }

        public IEnumerable<Doctor> GetDoctorsByExperience(int minYears)
        {
            return _doctors.Values
                .Where(d => d.YearsOfExperience >= minYears)
                .OrderByDescending(d => d.YearsOfExperience);
        }
    }
}
