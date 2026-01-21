using HospitalManagementSystem.Domain.Entities;

namespace HospitalManagementSystem.Domain.Repositories
{
    public interface IMedicalRecordRepository : IRepository<MedicalRecord>
    {
        // Custom query methods
        IEnumerable<MedicalRecord> GetRecordsByPatient(int patientId);
        IEnumerable<MedicalRecord> GetRecordsByDoctor(int doctorId);
        IEnumerable<MedicalRecord> GetRecordsByDateRange(DateTime startDate, DateTime endDate);
        MedicalRecord GetRecordByAppointment(int appointmentId);
    }
}
