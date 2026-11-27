using System.Text;
using ClinicApp.Events;

namespace ClinicApp.Utils
{
    public class ClinicLogger
    {
        private readonly string _logPath;

        public ClinicLogger(string logPath = "clinic.log")
        {
            _logPath = logPath;
        }

        public void LogInfo(string message)
        {
            WriteLog("INFO ", message);
        }

        public void LogWarning(string message)
        {
            WriteLog("WARN ", message);
        }

        public void LogError(string message)
        {
            WriteLog("ERROR", message);
        }

        public void OnPatientAdded(object? sender, PatientEventArgs e)
            => LogInfo($"Новий пацієнт #{e.PatientId}: {e.FullName}");

        public void OnAppointmentBooked(object? sender, AppointmentEventArgs e)
            => LogInfo($"Запис #{e.AppointmentId} створено: пацієнт #{e.PatientId} → лікар #{e.DoctorId}, {e.ScheduledAt:dd.MM.yyyy HH:mm}");

        public void OnUrgentAppointmentBooked(object? sender, AppointmentEventArgs e)
        {
            string details = string.IsNullOrWhiteSpace(e.Notes)
                ? $"запис #{e.AppointmentId} (пацієнт #{e.PatientId} → лікар #{e.DoctorId})"
                : $"запис #{e.AppointmentId}: {e.Notes}";
            LogWarning($"ТЕРМІНОВИЙ {details}");

            Directory.CreateDirectory("alerts");
            string alertFile = Path.Combine("alerts", $"urgent_{DateTime.Now:yyyy-MM-dd}.txt");
            string alertLine = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {details}{Environment.NewLine}";
            File.AppendAllText(alertFile, alertLine, Encoding.UTF8);
        }

        public void OnAppointmentCancelled(object? sender, AppointmentEventArgs e)
        {
            string reasonPart = string.IsNullOrWhiteSpace(e.Notes) ? "" : $" (причина: {e.Notes})";
            LogInfo($"Запис #{e.AppointmentId} скасовано{reasonPart}.");
        }

        public void OnAppointmentCompleted(object? sender, AppointmentEventArgs e)
            => LogInfo($"Запис #{e.AppointmentId} завершено.");

        public void OnPaymentReceived(object? sender, PaymentEventArgs e)
            => LogInfo($"Оплата запису #{e.AppointmentId}: {e.Amount:F2} грн");

        public void OnPlanCompleted(object? sender, TreatmentPlanEventArgs e)
            => LogInfo($"План лікування #{e.PlanId} (пацієнт #{e.PatientId}, діагноз: {e.Diagnosis}) завершено.");

        public string[] GetLastLines(int n)
        {
            if (n <= 0 || !Exists())
            {
                return Array.Empty<string>();
            }

            string[] lines = File.ReadAllLines(_logPath, Encoding.UTF8);
            int startIndex = lines.Length - n;
            if (startIndex < 0)
            {
                startIndex = 0;
            }

            int resultLength = lines.Length - startIndex;
            string[] result = new string[resultLength];
            for (int i = 0; i < resultLength; i++)
            {
                result[i] = lines[startIndex + i];
            }

            return result;
        }

        public void Clear()
        {
            if (Exists())
            {
                File.Delete(_logPath);
            }
        }

        public bool Exists()
        {
            return File.Exists(_logPath);
        }

        private void WriteLog(string level, string message)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string line = $"[{timestamp}] [{level}] {message}{Environment.NewLine}";
            File.AppendAllText(_logPath, line, Encoding.UTF8);
        }
    }
}