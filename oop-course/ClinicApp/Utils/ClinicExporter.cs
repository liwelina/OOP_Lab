using System.Text;
using ClinicApp.Interfaces;
using ClinicApp.Models;

namespace ClinicApp.Utils
{
    public class ClinicExporter
    {
        private readonly Clinic _clinic;
        private readonly string _baseDir;

        public ClinicExporter(Clinic clinic, string baseDir = "reports")
        {
            _clinic = clinic;
            _baseDir = baseDir;
        }

        public string ExportPatients()
        {
            string dir = GetDateDirectory();
            string path = Path.Combine(dir, "patients.txt");
            Patient[] patients = _clinic.Patients.GetAll();

            using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                WriteHeader(writer, "Пацієнти — звіт");

                for (int i = 0; i < patients.Length; i++)
                {
                    writer.WriteLine($"{i + 1,3}. {patients[i]}");
                }

                WriteFooter(writer, $"Всього: {patients.Length}");
            }

            return path;
        }

        public string ExportAppointments()
        {
            string dir = GetDateDirectory();
            string path = Path.Combine(dir, "appointments.txt");
            Appointment[] appointments = _clinic.Appointments.GetAll();

            using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                WriteHeader(writer, "Записи — звіт");

                for (int i = 0; i < appointments.Length; i++)
                {
                    writer.WriteLine($"{i + 1,3}. {appointments[i]}");
                }

                WriteFooter(writer, $"Всього: {appointments.Length}");
            }

            return path;
        }

        public string ExportBilling()
        {
            string dir = GetDateDirectory();
            string path = Path.Combine(dir, "billing.txt");
            IPayable[] unpaid = _clinic.Billing.GetAllUnpaid();
            decimal totalDebt = _clinic.Billing.GetTotalDebt();

            using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                WriteHeader(writer, "Рахунки (неоплачені) — звіт");

                for (int i = 0; i < unpaid.Length; i++)
                {
                    writer.WriteLine($"{i + 1,3}. {unpaid[i]}");
                }

                writer.WriteLine("----------------------------------------");
                writer.WriteLine($"Неоплачених записів: {unpaid.Length}");
                writer.WriteLine($"Загальний борг: {totalDebt:F2} грн");
            }

            return path;
        }

        public string ExportTreatmentPlans()
        {
            string dir = GetDateDirectory();
            string path = Path.Combine(dir, "treatment_plans.txt");
            TreatmentPlan[] plans = _clinic.TreatmentPlans.GetAll();

            using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                WriteHeader(writer, "Плани лікування — звіт");

                for (int i = 0; i < plans.Length; i++)
                {
                    writer.WriteLine($"{i + 1,3}. {plans[i]}");
                }

                WriteFooter(writer, $"Всього: {plans.Length}");
            }

            return path;
        }

        public string[] ExportAll()
        {
            string[] paths = new string[4];
            paths[0] = ExportPatients();
            paths[1] = ExportAppointments();
            paths[2] = ExportBilling();
            paths[3] = ExportTreatmentPlans();
            return paths;
        }

        private string GetDateDirectory()
        {
            string dateFolder = DateTime.Now.ToString("yyyy-MM-dd");
            string fullDir = Path.Combine(_baseDir, dateFolder);
            Directory.CreateDirectory(fullDir);
            return fullDir;
        }

        private static void WriteHeader(StreamWriter writer, string title)
        {
            string generatedAt = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
            writer.WriteLine($"=== {title} ===");
            writer.WriteLine($"Згенеровано: {generatedAt}");
            writer.WriteLine("----------------------------------------");
        }

        private static void WriteFooter(StreamWriter writer, string summary)
        {
            writer.WriteLine("----------------------------------------");
            writer.WriteLine(summary);
        }
    }
}
