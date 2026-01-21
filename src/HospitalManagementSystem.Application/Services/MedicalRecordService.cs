using HospitalManagementSystem.Domain.Entities;
using HospitalManagementSystem.Domain.Repositories;
using HospitalManagementSystem.Domain.Exceptions;
using HospitalManagementSystem.Application.Results;

namespace HospitalManagementSystem.Application.Services
{
    public class MedicalRecordService
    {
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IAppointmentRepository _appointmentRepository;

        public MedicalRecordService(
            IMedicalRecordRepository medicalRecordRepository,
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository,
            IAppointmentRepository appointmentRepository)
        {
            _medicalRecordRepository = medicalRecordRepository;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _appointmentRepository = appointmentRepository;
        }

        // Business Logic: Create Medical Record
        public OperationResult<MedicalRecord> CreateRecord(
            int patientId,
            int doctorId,
            string diagnosis,
            string prescription,
            string labResults = null,
            string notes = null,
            int? appointmentId = null)
        {
            try
            {
                // 1. Validate patient
                var patient = _patientRepository.GetById(patientId);
                if (patient == null)
                    throw new PatientNotFoundException(patientId);

                // 2. Validate doctor
                var doctor = _doctorRepository.GetById(doctorId);
                if (doctor == null)
                    throw new DoctorNotFoundException(doctorId);

                // 3. Validate appointment if provided
                if (appointmentId.HasValue)
                {
                    var appointment = _appointmentRepository.GetById(appointmentId.Value);
                    if (appointment == null)
                        throw new AppointmentNotFoundException(appointmentId.Value);
                }

                // 4. Create medical record
                var record = new MedicalRecord
                {
                    Patient = patient,
                    Doctor = doctor,
                    Diagnosis = diagnosis,
                    Prescription = prescription,
                    LabResults = labResults,
                    Notes = notes,
                    AppointmentId = appointmentId,
                    Date = DateTime.Now
                };

                // 5. Validate
                if (!record.IsValid())
                    return OperationResult<MedicalRecord>.Failure("Invalid medical record data");

                // 6. Save
                _medicalRecordRepository.Add(record);

                return OperationResult<MedicalRecord>.Success(
                    record,
                    "Medical record created successfully"
                );
            }
            catch (DomainException ex)
            {
                return OperationResult<MedicalRecord>.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                return OperationResult<MedicalRecord>.Failure($"An error occurred: {ex.Message}");
            }
        }

        // Business Logic: Get Patient History
        public List<MedicalRecord> GetPatientHistory(int patientId)
        {
            return _medicalRecordRepository.GetRecordsByPatient(patientId).ToList();
        }

        // Business Logic: Get Records by Doctor
        public List<MedicalRecord> GetRecordsByDoctor(int doctorId)
        {
            return _medicalRecordRepository.GetRecordsByDoctor(doctorId).ToList();
        }

        // Business Logic: Get Record by Appointment
        public MedicalRecord GetRecordByAppointment(int appointmentId)
        {
            return _medicalRecordRepository.GetRecordByAppointment(appointmentId);
        }

        // Business Logic: Update Medical Record
        public OperationResult<MedicalRecord> UpdateRecord(
            int recordId,
            string diagnosis = null,
            string prescription = null,
            string labResults = null,
            string notes = null)
        {
            try
            {
                var record = _medicalRecordRepository.GetById(recordId);
                if (record == null)
                    return OperationResult<MedicalRecord>.Failure("Medical record not found");

                // Update fields if provided
                if (!string.IsNullOrWhiteSpace(diagnosis))
                    record.Diagnosis = diagnosis;

                if (!string.IsNullOrWhiteSpace(prescription))
                    record.Prescription = prescription;

                if (labResults != null)
                    record.LabResults = labResults;

                if (notes != null)
                    record.Notes = notes;

                _medicalRecordRepository.Update(record);

                return OperationResult<MedicalRecord>.Success(
                    record,
                    "Medical record updated successfully"
                );
            }
            catch (Exception ex)
            {
                return OperationResult<MedicalRecord>.Failure($"Update failed: {ex.Message}");
            }
        }

        // Business Logic: Search Records by Date Range
        public List<MedicalRecord> SearchRecordsByDateRange(DateTime startDate, DateTime endDate)
        {
            return _medicalRecordRepository.GetRecordsByDateRange(startDate, endDate).ToList();
        }
    }
}
