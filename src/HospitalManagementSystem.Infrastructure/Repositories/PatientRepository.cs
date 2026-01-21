using HospitalManagementSystem.Domain.Entities;
using HospitalManagementSystem.Domain.Repositories;

namespace HospitalManagementSystem.Infrastructure.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly Dictionary<int, Patient> _patients;
        private int _nextId;

        public PatientRepository()
        {
            _patients = new Dictionary<int, Patient>();
            _nextId = 1;
        }

        // Basic CRUD operations
        public Patient GetById(int id)
        {
            return _patients.ContainsKey(id) ? _patients[id] : null;
        }

        public IEnumerable<Patient> GetAll()
        {
            return _patients.Values;
        }

        public void Add(Patient entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.Id = _nextId++;
            _patients[entity.Id] = entity;
        }

        public void Update(Patient entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (_patients.ContainsKey(entity.Id))
            {
                _patients[entity.Id] = entity;
            }
        }

        public void Delete(int id)
        {
            if (_patients.ContainsKey(id))
            {
                _patients.Remove(id);
            }
        }

        // Custom query methods
        public IEnumerable<Patient> GetPatientsByBloodType(string bloodType)
        {
            return _patients.Values
                .Where(p => p.BloodType.Equals(bloodType, StringComparison.OrdinalIgnoreCase));
        }

        public Patient GetByMedicalRecordNumber(string mrn)
        {
            return _patients.Values
                .FirstOrDefault(p => p.MedicalRecordNumber == mrn);
        }

        public IEnumerable<Patient> GetPatientsWithAllergy(string allergen)
        {
            return _patients.Values
                .Where(p => p.HasAllergy(allergen));
        }

        public IEnumerable<Patient> GetPatientsWithUpcomingAppointments()
        {
            return _patients.Values
                .Where(p => p.GetUpcomingAppointments().Any());
        }
    }
}
