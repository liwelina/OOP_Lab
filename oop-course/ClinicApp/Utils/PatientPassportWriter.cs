using System.Text;
using ClinicApp.Events;
using ClinicApp.Models;

namespace ClinicApp.Utils
{
    public class PatientPassportWriter
    {
        private readonly Clinic _clinic;
        private readonly string _passportDirectory;

        public PatientPassportWriter(Clinic clinic, string passportDirectory = "patients")
        {
            _clinic = clinic;
            _passportDirectory = passportDirectory;
            Directory.CreateDirectory(_passportDirectory);
        }

        public void OnPatientAdded(object? sender, PatientEventArgs e)
        {
            WritePassport(e.PatientId);
        }

        public void OnAppointmentCompleted(object? sender, AppointmentEventArgs e)
        {
            WritePassport(e.PatientId);
        }

        public void OnPlanCompleted(object? sender, TreatmentPlanEventArgs e)
        {
            WritePassport(e.PatientId);
        }

        private void WritePassport(int patientId)
        {
            Patient? patient = _clinic.Patients.FindById(patientId);
            if (patient == null)
            {
                return;
            }

            string filePath = Path.Combine(_passportDirectory, $"passport_{patient.Id}.txt");

            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                writer.WriteLine($"=== Паспорт пацієнта #{patient.Id} ===");
                writer.WriteLine($"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm}");
                writer.WriteLine();

                writer.WriteLine($"Ім'я: {patient.FullName}");
                writer.WriteLine($"Дата народження: {patient.DateOfBirth:dd.MM.yyyy} ({patient.Age} років)");
                writer.WriteLine($"Група крові: {patient.BloodType}   Телефон: {patient.Phone}");
                writer.WriteLine();

                writer.WriteLine("-- Діагнози --");
                writer.WriteLine();

                writer.WriteLine("-- Аналізи --");
                writer.WriteLine();

                writer.WriteLine("-- Рецепти --");
                writer.WriteLine();

                writer.WriteLine("-- Записи --");
                Appointment[] appointments = _clinic.Appointments.GetByPatient(patient.Id);
                for (int i = 0; i < appointments.Length; i++)
                {
                    writer.WriteLine(appointments[i].ToString());
                }
                writer.WriteLine();

                writer.WriteLine("-- Плани лікування --");
                TreatmentPlan[] plans = _clinic.TreatmentPlans.GetByPatient(patient.Id);
                for (int i = 0; i < plans.Length; i++)
                {
                    writer.WriteLine(plans[i].ToString());
                }
                writer.WriteLine();

                decimal debt = _clinic.Billing.GetPatientDebt(patient.Id);
                writer.WriteLine($"Заборгованість: {debt:F2} грн");
            }
        }
    }
}