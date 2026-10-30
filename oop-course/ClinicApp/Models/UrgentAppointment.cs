namespace ClinicApp.Models
{
    public class UrgentAppointment : Appointment
    {
        public string UrgencyNote { get; }

        public UrgentAppointment(int patientId, int doctorId, DateTime scheduledAt, string urgencyNote = "", int durationMinutes = 30)
            : base(patientId, doctorId, scheduledAt, durationMinutes)
        {
            UrgencyNote = urgencyNote;
        }

        public override decimal GetCost()
        {
            // Базова ціна батька помножена на 1.5 (на 50% дорожче)
            return base.GetCost() * 1.5m;
        }

        public sealed override string GetDescription()
        {
            if (string.IsNullOrWhiteSpace(UrgencyNote))
            {
                return "Терміновий";
            }
            return $"Терміновий ({UrgencyNote})";
        }

        // Приховування методу батька через new
        public new int GetPriority()
        {
            return 1;
        }
    }
}