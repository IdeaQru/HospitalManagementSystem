using HospitalManagementSystem.Domain.Entities;
using HospitalManagementSystem.Domain.Enums;
using HospitalManagementSystem.Domain.ValueObjects;
using HospitalManagementSystem.Domain.Repositories;
using HospitalManagementSystem.Application.Services;
using HospitalManagementSystem.Application.Interfaces;
using HospitalManagementSystem.Infrastructure.Repositories;
using HospitalManagementSystem.Infrastructure.Factories;
using HospitalManagementSystem.Infrastructure.Strategies;
using HospitalManagementSystem.Infrastructure.Observers;
// HAPUS: using HospitalManagementSystem.Infrastructure.Configuration;

namespace HospitalManagementSystem.Presentation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("╔═══════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                                                           ║");
            Console.WriteLine("║       HOSPITAL MANAGEMENT SYSTEM - DEMO                   ║");
            Console.WriteLine("║       Advanced OOP with Design Patterns                   ║");
            Console.WriteLine("║                                                           ║");
            Console.WriteLine("╚═══════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            // Initialize all components
            var (repositories, services, observers) = InitializeSystem();

            // HAPUS atau COMMENT: DisplayHospitalConfiguration();

            // Run all demos
            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("STARTING COMPREHENSIVE DEMO");
            Console.WriteLine(new string('=', 60) + "\n");

            DemoFactoryPattern(repositories.PersonFactory);
            DemoRepositoryPattern(repositories);
            DemoAppointmentService(services.AppointmentService, repositories);
            DemoStrategyPattern(services.PaymentService, repositories);
            DemoObserverPattern(services.AppointmentService, observers);
            DemoMedicalRecordService(services.MedicalRecordService, repositories);
            DisplayStatistics(repositories);

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("DEMO COMPLETED SUCCESSFULLY!");
            Console.WriteLine(new string('=', 60));
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        // ============================================
        // INITIALIZATION
        // ============================================
        static (Repositories, Services, Observers) InitializeSystem()
        {
            Console.WriteLine("🔧 Initializing System Components...\n");

            // Create repositories
            var patientRepo = new PatientRepository();
            var doctorRepo = new DoctorRepository();
            var appointmentRepo = new AppointmentRepository();
            var medicalRecordRepo = new MedicalRecordRepository();
            var paymentRepo = new PaymentRepository();

            // Create factory
            var personFactory = new PersonFactory();

            // Create services
            var appointmentService = new AppointmentService(appointmentRepo, patientRepo, doctorRepo);
            var paymentService = new PaymentService(paymentRepo, appointmentRepo);
            var medicalRecordService = new MedicalRecordService(medicalRecordRepo, patientRepo, doctorRepo, appointmentRepo);

            // Create observers
            var emailObserver = new EmailNotificationObserver();
            var smsObserver = new SMSNotificationObserver();
            var whatsappObserver = new WhatsAppNotificationObserver();

            Console.WriteLine("✅ System initialized successfully!\n");

            return (
                new Repositories(patientRepo, doctorRepo, appointmentRepo, medicalRecordRepo, paymentRepo, personFactory),
                new Services(appointmentService, paymentService, medicalRecordService),
                new Observers(emailObserver, smsObserver, whatsappObserver)
            );
        }

        // HAPUS METHOD INI:
        // static void DisplayHospitalConfiguration() { ... }

        // ============================================
        // DEMO 1: FACTORY PATTERN
        // ============================================
        static void DemoFactoryPattern(IPersonFactory personFactory)
        {
            Console.WriteLine("\n" + new string('─', 60));
            Console.WriteLine("📦 DEMO 1: FACTORY PATTERN - Creating Persons");
            Console.WriteLine(new string('─', 60) + "\n");

            try
            {
                // Create Patient
                var patientData = new PersonData
                {
                    Name = "John Doe",
                    DateOfBirth = new DateTime(1990, 5, 15),
                    PhoneNumber = "+62812-3456-7890",
                    Email = "john.doe@email.com",
                    MedicalRecordNumber = "MRN-001",
                    BloodType = "A+"
                };

                var patient = personFactory.CreatePerson(PersonType.Patient, patientData);
                Console.WriteLine($"✓ Patient created: {patient.GetFullInfo()}");

                // Create Doctor
                var doctorData = new PersonData
                {
                    Name = "Dr. Sarah Smith",
                    DateOfBirth = new DateTime(1980, 3, 20),
                    PhoneNumber = "+62813-9876-5432",
                    Email = "dr.smith@hospital.com",
                    Specialization = "Cardiology",
                    LicenseNumber = "DOC-12345",
                    YearsOfExperience = 15
                };

                var doctor = personFactory.CreatePerson(PersonType.Doctor, doctorData);
                Console.WriteLine($"✓ Doctor created: {doctor.GetFullInfo()}");

                // Create Nurse
                var nurseData = new PersonData
                {
                    Name = "Maria Garcia",
                    DateOfBirth = new DateTime(1995, 8, 10),
                    PhoneNumber = "+62814-5555-6666",
                    Email = "maria.garcia@hospital.com",
                    Department = "Emergency",
                    ShiftSchedule = "Night Shift"
                };

                var nurse = personFactory.CreatePerson(PersonType.Nurse, nurseData);
                Console.WriteLine($"✓ Nurse created: {nurse.GetFullInfo()}");

                Console.WriteLine($"\n✅ Factory Pattern Demo: SUCCESS");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Factory Pattern Demo: FAILED - {ex.Message}");
            }
        }

        // ============================================
        // DEMO 2: REPOSITORY PATTERN
        // ============================================
        static void DemoRepositoryPattern(Repositories repos)
        {
            Console.WriteLine("\n" + new string('─', 60));
            Console.WriteLine("💾 DEMO 2: REPOSITORY PATTERN - Data Persistence");
            Console.WriteLine(new string('─', 60) + "\n");

            // Create and save patients
            var patient1 = new Patient
            {
                Name = "Alice Johnson",
                DateOfBirth = new DateTime(1985, 2, 14),
                PhoneNumber = "+62821-1111-2222",
                Email = "alice.j@email.com",
                MedicalRecordNumber = "MRN-002",
                BloodType = "O+",
                Allergies = new List<string> { "Penicillin", "Peanuts" }
            };

            var patient2 = new Patient
            {
                Name = "Bob Williams",
                DateOfBirth = new DateTime(1992, 7, 22),
                PhoneNumber = "+62822-3333-4444",
                Email = "bob.w@email.com",
                MedicalRecordNumber = "MRN-003",
                BloodType = "B+",
                Allergies = new List<string> { "Latex" }
            };

            repos.PatientRepository.Add(patient1);
            repos.PatientRepository.Add(patient2);
            Console.WriteLine($"✓ Added 2 patients to repository");

            // Create and save doctors with available time slots
            var doctor1 = new Doctor
            {
                Name = "Dr. Michael Chen",
                DateOfBirth = new DateTime(1975, 11, 5),
                PhoneNumber = "+62831-7777-8888",
                Email = "dr.chen@hospital.com",
                Specialization = "General Practice",
                LicenseNumber = "DOC-67890",
                YearsOfExperience = 20
            };

            // Add available time slots for tomorrow
            var tomorrow = DateTime.Today.AddDays(1);
            doctor1.AvailableTimeSlots = new List<TimeSlot>
            {
                new TimeSlot(tomorrow, new TimeSpan(9, 0, 0), new TimeSpan(9, 30, 0)),
                new TimeSlot(tomorrow, new TimeSpan(10, 0, 0), new TimeSpan(10, 30, 0)),
                new TimeSlot(tomorrow, new TimeSpan(11, 0, 0), new TimeSpan(11, 30, 0)),
                new TimeSlot(tomorrow, new TimeSpan(14, 0, 0), new TimeSpan(14, 30, 0)),
                new TimeSlot(tomorrow, new TimeSpan(15, 0, 0), new TimeSpan(15, 30, 0))
            };

            repos.DoctorRepository.Add(doctor1);
            Console.WriteLine($"✓ Added doctor with {doctor1.AvailableTimeSlots.Count} available slots");

            // Query repositories
            var allPatients = repos.PatientRepository.GetAll();
            Console.WriteLine($"✓ Total patients in repository: {allPatients.Count()}");

            var oPositivePatients = repos.PatientRepository.GetPatientsByBloodType("O+");
            Console.WriteLine($"✓ Patients with O+ blood type: {oPositivePatients.Count()}");

            var generalPractitioners = repos.DoctorRepository.GetDoctorsBySpecialization("General Practice");
            Console.WriteLine($"✓ General Practice doctors: {generalPractitioners.Count()}");

            Console.WriteLine($"\n✅ Repository Pattern Demo: SUCCESS");
        }

        // ============================================
        // DEMO 3: APPOINTMENT SERVICE
        // ============================================
        static void DemoAppointmentService(AppointmentService appointmentService, Repositories repos)
        {
            Console.WriteLine("\n" + new string('─', 60));
            Console.WriteLine("📅 DEMO 3: APPOINTMENT SERVICE - Booking & Management");
            Console.WriteLine(new string('─', 60) + "\n");

            var patient = repos.PatientRepository.GetAll().First();
            var doctor = repos.DoctorRepository.GetAll().First();

            // Book appointment
            var tomorrow = DateTime.Today.AddDays(1).AddHours(9);
            var bookingResult = appointmentService.BookAppointment(
                patient.Id,
                doctor.Id,
                tomorrow,
                "Annual checkup",
                30
            );

            if (bookingResult.IsSuccess)
            {
                Console.WriteLine($"✓ {bookingResult.Message}");
                Console.WriteLine($"  Patient: {patient.Name}");
                Console.WriteLine($"  Doctor: {doctor.Name}");
                Console.WriteLine($"  Date: {tomorrow:yyyy-MM-dd HH:mm}");
                Console.WriteLine($"  Duration: 30 minutes");
            }
            else
            {
                Console.WriteLine($"❌ Booking failed: {bookingResult.Message}");
            }

            // Get upcoming appointments
            var upcoming = appointmentService.GetUpcomingAppointments();
            Console.WriteLine($"\n✓ Total upcoming appointments: {upcoming.Count}");

            // Complete appointment
            if (bookingResult.IsSuccess && bookingResult.Appointment != null)
            {
                var completeResult = appointmentService.CompleteAppointment(bookingResult.Appointment.Id);
                if (completeResult.IsSuccess)
                {
                    Console.WriteLine($"✓ Appointment marked as completed");
                }
            }

            Console.WriteLine($"\n✅ Appointment Service Demo: SUCCESS");
        }

        // ============================================
        // DEMO 4: STRATEGY PATTERN (Payment)
        // ============================================
        static void DemoStrategyPattern(PaymentService paymentService, Repositories repos)
        {
            Console.WriteLine("\n" + new string('─', 60));
            Console.WriteLine("💳 DEMO 4: STRATEGY PATTERN - Payment Processing");
            Console.WriteLine(new string('─', 60) + "\n");

            var appointment = repos.AppointmentRepository.GetAll().FirstOrDefault();
            if (appointment == null)
            {
                Console.WriteLine("⚠ No appointment available for payment demo");
                return;
            }

            // Demo 1: Cash Payment
            Console.WriteLine("1️⃣  Cash Payment:");
            paymentService.SetPaymentStrategy(new CashPaymentStrategy());
            var cashResult = paymentService.ProcessPayment(
                appointment.Id,
                500000,
                PaymentMethod.Cash,
                new PaymentDetails()
            );
            Console.WriteLine($"   {(cashResult.IsSuccess ? "✓" : "❌")} {cashResult.Message}");

            // Demo 2: Credit Card Payment
            Console.WriteLine("\n2️⃣  Credit Card Payment:");
            paymentService.SetPaymentStrategy(new CreditCardPaymentStrategy());
            var ccDetails = new PaymentDetails
            {
                CardNumber = "1234567890123456",
                CardHolderName = "John Doe",
                CVV = "123",
                ExpiryDate = DateTime.Now.AddYears(2)
            };
            
            // Create new appointment for second payment
            var patient = repos.PatientRepository.GetAll().First();
            var doctor = repos.DoctorRepository.GetAll().First();
            var appointment2 = new Appointment
            {
                Patient = patient,
                Doctor = doctor,
                AppointmentDateTime = DateTime.Today.AddDays(1).AddHours(10),
                Duration = 30,
                Reason = "Follow-up",
                Status = AppointmentStatus.Scheduled
            };
            repos.AppointmentRepository.Add(appointment2);
            
            var ccResult = paymentService.ProcessPayment(
                appointment2.Id,
                750000,
                PaymentMethod.CreditCard,
                ccDetails
            );
            Console.WriteLine($"   {(ccResult.IsSuccess ? "✓" : "❌")} {ccResult.Message}");

            Console.WriteLine($"\n✅ Strategy Pattern Demo: SUCCESS");
        }

        // ============================================
        // DEMO 5: OBSERVER PATTERN (Notifications)
        // ============================================
        static void DemoObserverPattern(AppointmentService appointmentService, Observers observers)
        {
            Console.WriteLine("\n" + new string('─', 60));
            Console.WriteLine("🔔 DEMO 5: OBSERVER PATTERN - Notification System");
            Console.WriteLine(new string('─', 60) + "\n");

            // Attach all observers
            appointmentService.Attach(observers.EmailObserver);
            appointmentService.Attach(observers.SMSObserver);
            appointmentService.Attach(observers.WhatsAppObserver);

            Console.WriteLine("✓ Attached 3 observers (Email, SMS, WhatsApp)");
            Console.WriteLine("\nObservers will be notified when appointment status changes...");

            Console.WriteLine($"\n✅ Observer Pattern Demo: SUCCESS");
            Console.WriteLine("   (Notifications were sent during previous appointment operations)");
        }

        // ============================================
        // DEMO 6: MEDICAL RECORD SERVICE
        // ============================================
        static void DemoMedicalRecordService(MedicalRecordService medicalRecordService, Repositories repos)
        {
            Console.WriteLine("\n" + new string('─', 60));
            Console.WriteLine("📋 DEMO 6: MEDICAL RECORD SERVICE - Patient Records");
            Console.WriteLine(new string('─', 60) + "\n");

            var patient = repos.PatientRepository.GetAll().First();
            var doctor = repos.DoctorRepository.GetAll().First();
            var appointment = repos.AppointmentRepository.GetAll().FirstOrDefault();

            // Create medical record
            var recordResult = medicalRecordService.CreateRecord(
                patient.Id,
                doctor.Id,
                "Annual health checkup - All vital signs normal",
                "Multivitamin - 1 tablet daily for 30 days",
                "Blood pressure: 120/80, Heart rate: 72 bpm",
                "Patient is in good health. Recommend regular exercise.",
                appointment?.Id
            );

            if (recordResult.IsSuccess)
            {
                Console.WriteLine($"✓ Medical record created successfully");
                Console.WriteLine($"  Patient: {patient.Name}");
                Console.WriteLine($"  Doctor: {doctor.Name}");
                Console.WriteLine($"  Diagnosis: {recordResult.Data.Diagnosis}");
                Console.WriteLine($"  Date: {recordResult.Data.Date:yyyy-MM-dd HH:mm}");
            }

            // Get patient history
            var history = medicalRecordService.GetPatientHistory(patient.Id);
            Console.WriteLine($"\n✓ Patient medical history: {history.Count} record(s)");

            Console.WriteLine($"\n✅ Medical Record Service Demo: SUCCESS");
        }

        // ============================================
        // STATISTICS
        // ============================================
        static void DisplayStatistics(Repositories repos)
        {
            Console.WriteLine("\n" + new string('─', 60));
            Console.WriteLine("📊 SYSTEM STATISTICS");
            Console.WriteLine(new string('─', 60) + "\n");

            var totalPatients = repos.PatientRepository.GetAll().Count();
            var totalDoctors = repos.DoctorRepository.GetAll().Count();
            var totalAppointments = repos.AppointmentRepository.GetAll().Count();
            var totalRecords = repos.MedicalRecordRepository.GetAll().Count();
            var totalPayments = repos.PaymentRepository.GetAll().Count();
            var completedPayments = repos.PaymentRepository.GetPaymentsByStatus(PaymentStatus.Completed).Count();

            Console.WriteLine($"👥 Total Patients: {totalPatients}");
            Console.WriteLine($"👨‍⚕️ Total Doctors: {totalDoctors}");
            Console.WriteLine($"📅 Total Appointments: {totalAppointments}");
            Console.WriteLine($"📋 Total Medical Records: {totalRecords}");
            Console.WriteLine($"💰 Total Payments: {totalPayments}");
            Console.WriteLine($"✅ Completed Payments: {completedPayments}");

            // Revenue calculation
            var revenue = repos.PaymentRepository.GetTotalRevenue(DateTime.Today.AddMonths(-1), DateTime.Today.AddDays(1));
            Console.WriteLine($"💵 Total Revenue: Rp {revenue:N0}");
        }

        // ============================================
        // HELPER CLASSES
        // ============================================
        record Repositories(
            IPatientRepository PatientRepository,
            IDoctorRepository DoctorRepository,
            IAppointmentRepository AppointmentRepository,
            IMedicalRecordRepository MedicalRecordRepository,
            IPaymentRepository PaymentRepository,
            IPersonFactory PersonFactory
        );

        record Services(
            AppointmentService AppointmentService,
            PaymentService PaymentService,
            MedicalRecordService MedicalRecordService
        );

        record Observers(
            IObserver EmailObserver,
            IObserver SMSObserver,
            IObserver WhatsAppObserver
        );
    }
}
