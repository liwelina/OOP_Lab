using ClinicApp.Enums;
using ClinicApp.Interfaces;
using ClinicApp.Utils;

namespace ClinicApp.Models
{
    public class Appointment : IPayable, ICancellable
    {
        private static int _nextId = 1;

        public int Id { get; private set; }
        public int PatientId { get; }
        public int DoctorId { get; }
        public DateTime ScheduledAt { get; set; }

        private int _durationMinutes;
        public int DurationMinutes
        {
            get => _durationMinutes;
            set
            {
                ClinicValidator.ValidatePositive(value, nameof(DurationMinutes));
                _durationMinutes = value;
            }
        }

        public AppointmentStatus Status { get; private set; }
        public string Notes { get; private set; }

      
        private bool _isPaid;
        public bool IsPaid => _isPaid;

        public bool IsCancelled => Status == AppointmentStatus.Cancelled;
        public string CancellationReason => IsCancelled ? Notes : "";

        public DateTime EndsAt => ScheduledAt.AddMinutes(DurationMinutes);
        public bool IsUpcoming => ScheduledAt > DateTime.Now && Status == AppointmentStatus.Scheduled;

        
        public decimal DiscountMultiplier { get; set; } = 1.0m;

        public Appointment(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30)
        {
            PatientId = patientId;
            DoctorId = doctorId;
            ScheduledAt = scheduledAt;
            DurationMinutes = durationMinutes;
            Status = AppointmentStatus.Scheduled;
            Notes = "";
            Id = _nextId++;
            _isPaid = false;
        }

        public void MarkPaid()
        {
            if (!IsCancelled)
            {
                _isPaid = true;
            }
        }

        // Віртуальний метод для вартості з урахуванням знижки (Варіант А)
        public virtual decimal GetCost()
        {
            // Базова ціна помножена на множник знижки. 
            // Підкласи викликають base.GetCost(), тому знижка автоматично застосується до термінових та спеціалізованих прийомів.
            return DurationMinutes * 10m * DiscountMultiplier;
        }

        public virtual string GetDescription()
        {
            return "Прийом";
        }

        public int GetPriority()
        {
            return 3;
        }

        public bool Cancel(string reason = "")
        {
            if (Status != AppointmentStatus.Scheduled)
            {
                return false;
            }

            Status = AppointmentStatus.Cancelled;
            if (!string.IsNullOrEmpty(reason))
            {
                Notes = reason;
            }
            return true;
        }

        public bool Complete()
        {
            if (Status != AppointmentStatus.Scheduled)
            {
                return false;
            }

            Status = AppointmentStatus.Completed;
            return true;
        }

        public override string ToString()
        {
            string result = $"[{Id}] {GetDescription()} | Patient #{PatientId} > Doctor #{DoctorId} | {ScheduledAt:dd.MM.yyyy HH:mm}–{EndsAt:HH:mm} | {Status}";
            if (Notes.Length > 0)
            {
                result += $" | {Notes}";
            }
            result += $" | {GetCost().ToString("F2")} грн";
            return result;
        }
    }
}