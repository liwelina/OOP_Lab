using ClinicApp.Models;

namespace ClinicApp.Managers
{
    public class AnalyticsManager
    {
        private readonly AppointmentManager _appointments;
        private readonly DoctorManager _doctors;
        private readonly PatientManager _patients;

        public AnalyticsManager(AppointmentManager appointments, DoctorManager doctors, PatientManager patients)
        {
            _appointments = appointments;
            _doctors = doctors;
            _patients = patients;
        }

        public IEnumerable<DoctorStats> ComputeDoctorStats()
        {
            Doctor[] doctors = _doctors.GetAll();
            Appointment[] appointments = _appointments.GetAll();

            foreach (var doc in doctors)
            {
                int count = 0;
                decimal revenue = 0m;
                DateTime lastDate = DateTime.MinValue;

                foreach (var app in appointments)
                {
                    if (app.DoctorId == doc.Id)
                    {
                        count++;
                        revenue += app.GetCost();

                        // Використовуємо правильну властивість ScheduledAt
                        if (app.ScheduledAt > lastDate)
                        {
                            lastDate = app.ScheduledAt;
                        }
                    }
                }

                yield return new DoctorStats(doc.Id, doc.FullName, count, revenue, lastDate);
            }
        }

        public IEnumerable<PatientStats> ComputePatientStats()
        {
            Patient[] patients = _patients.GetAll();
            Appointment[] appointments = _appointments.GetAll();

            foreach (var pat in patients)
            {
                int count = 0;
                decimal spent = 0m;
                DateTime lastDate = DateTime.MinValue;

                foreach (var app in appointments)
                {
                    if (app.PatientId == pat.Id)
                    {
                        count++;
                        spent += app.GetCost();

                        if (app.ScheduledAt > lastDate)
                        {
                            lastDate = app.ScheduledAt;
                        }
                    }
                }

                yield return new PatientStats(pat.Id, pat.FullName, count, spent, lastDate);
            }
        }
    }
}