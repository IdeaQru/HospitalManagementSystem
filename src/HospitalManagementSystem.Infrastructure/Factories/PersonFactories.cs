using HospitalManagementSystem.Domain.Entities;
using HospitalManagementSystem.Domain.Enums;
using HospitalManagementSystem.Domain.ValueObjects;
using HospitalManagementSystem.Application.Interfaces;

namespace HospitalManagementSystem.Infrastructure.Factories
{
    public class PersonFactory : IPersonFactory
    {
        public Person CreatePerson(PersonType type, PersonData data)
        {
            // Validation umum
            ValidatePersonData(data);

            // Pattern matching untuk create person berdasarkan type
            return type switch
            {
                PersonType.Patient => CreatePatient(data),
                PersonType.Doctor => CreateDoctor(data),
                PersonType.Nurse => CreateNurse(data),
                _ => throw new ArgumentException($"Unknown person type: {type}")
            };
        }

        private void ValidatePersonData(PersonData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (string.IsNullOrWhiteSpace(data.Name))
                throw new ArgumentException("Name is required");

            if (string.IsNullOrWhiteSpace(data.Email))
                throw new ArgumentException("Email is required");

            if (data.DateOfBirth >= DateTime.Today)
                throw new ArgumentException("Date of birth must be in the past");
        }

        private Patient CreatePatient(PersonData data)
        {
            if (string.IsNullOrWhiteSpace(data.MedicalRecordNumber))
                throw new ArgumentException("Medical Record Number is required for Patient");

            if (string.IsNullOrWhiteSpace(data.BloodType))
                throw new ArgumentException("Blood Type is required for Patient");

            return new Patient
            {
                Name = data.Name,
                DateOfBirth = data.DateOfBirth,
                PhoneNumber = data.PhoneNumber,
                Email = data.Email,
                MedicalRecordNumber = data.MedicalRecordNumber,
                BloodType = data.BloodType,
                Allergies = data.Allergies ?? new List<string>()
            };
        }

        private Doctor CreateDoctor(PersonData data)
        {
            if (string.IsNullOrWhiteSpace(data.Specialization))
                throw new ArgumentException("Specialization is required for Doctor");

            if (string.IsNullOrWhiteSpace(data.LicenseNumber))
                throw new ArgumentException("License Number is required for Doctor");

            if (data.YearsOfExperience < 0)
                throw new ArgumentException("Years of Experience cannot be negative");

            return new Doctor
            {
                Name = data.Name,
                DateOfBirth = data.DateOfBirth,
                PhoneNumber = data.PhoneNumber,
                Email = data.Email,
                Specialization = data.Specialization,
                LicenseNumber = data.LicenseNumber,
                YearsOfExperience = data.YearsOfExperience
            };
        }

        private Nurse CreateNurse(PersonData data)
        {
            if (string.IsNullOrWhiteSpace(data.Department))
                throw new ArgumentException("Department is required for Nurse");

            return new Nurse
            {
                Name = data.Name,
                DateOfBirth = data.DateOfBirth,
                PhoneNumber = data.PhoneNumber,
                Email = data.Email,
                Department = data.Department,
                ShiftSchedule = data.ShiftSchedule ?? "Day Shift"
            };
        }
    }
}
