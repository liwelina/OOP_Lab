using ClinicApp.Enums;
using ClinicApp.Interfaces;
using ClinicApp.Models;
using ClinicApp.Utils;

namespace ClinicApp.Models
{
    public class Appointment : IPayable
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

        // Реалізація IPayable: приватне поле та властивість IsPaid
        private bool _isPaid;
        public bool IsPaid => _isPaid;

        public DateTime EndsAt => ScheduledAt.AddMinutes(DurationMinutes);
        public bool IsUpcoming => ScheduledAt > DateTime.Now && Status == AppointmentStatus.Scheduled;

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

        // Реалізація методів IPayable
        public decimal GetCost()
        {
            // 10 грн за хвилину тривалості
            return DurationMinutes * 10m;
        }

        public void MarkPaid()
        {
            // Якщо запис скасований — нічого не робимо (тиха відмова)
            if (Status != AppointmentStatus.Cancelled)
            {
                _isPaid = true;
            }
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
            string result = $"[{Id}] Patient #{PatientId} > Doctor #{DoctorId} | {ScheduledAt:dd.MM.yyyy HH:mm}–{EndsAt:HH:mm} | {Status}";
            if (Notes.Length > 0)
            {
                result += $" | {Notes}";
            }
            return result;
        }
    }
}