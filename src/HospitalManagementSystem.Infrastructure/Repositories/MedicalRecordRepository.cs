using HospitalManagementSystem.Domain.Entities;
using HospitalManagementSystem.Domain.Repositories;

namespace HospitalManagementSystem.Infrastructure.Repositories
{
    public class MedicalRecordRepository : IMedicalRecordRepository
    {
        private readonly Dictionary<int, MedicalRecord> _records;
        private int _nextId;

        public MedicalRecordRepository()
        {
            _records = new Dictionary<int, MedicalRecord>();
            _nextId = 1;
        }

        public MedicalRecord GetById(int id)
        {
            return _records.ContainsKey(id) ? _records[id] : null;
        }

        public IEnumerable<MedicalRecord> GetAll()
        {
            return _records.Values;
        }

        public void Add(MedicalRecord entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.Id = _nextId++;
            _records[entity.Id] = entity;
        }

        public void Update(MedicalRecord entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (_records.ContainsKey(entity.Id))
            {
                _records[entity.Id] = entity;
            }
        }

        public void Delete(int id)
        {
            if (_records.ContainsKey(id))
            {
                _records.Remove(id);
            }
        }

        // Custom query methods
        public IEnumerable<MedicalRecord> GetRecordsByPatient(int patientId)
        {
            return _records.Values
                .Where(r => r.Patient != null && r.Patient.Id == patientId)
                .OrderByDescending(r => r.Date);
        }

        public IEnumerable<MedicalRecord> GetRecordsByDoctor(int doctorId)
        {
            return _records.Values
                .Where(r => r.Doctor != null && r.Doctor.Id == doctorId)
                .OrderByDescending(r => r.Date);
        }

        public IEnumerable<MedicalRecord> GetRecordsByDateRange(DateTime startDate, DateTime endDate)
        {
            return _records.Values
                .Where(r => r.Date >= startDate && r.Date <= endDate)
                .OrderByDescending(r => r.Date);
        }

        public MedicalRecord GetRecordByAppointment(int appointmentId)
        {
            return _records.Values
                .FirstOrDefault(r => r.AppointmentId == appointmentId);
        }
    }
}
