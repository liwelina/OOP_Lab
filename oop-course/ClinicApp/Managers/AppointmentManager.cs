using ClinicApp.Enums;
using ClinicApp.Interfaces;
using ClinicApp.Models;

namespace ClinicApp.Managers
{
    public class AppointmentManager
    {
        private const int MaxAppointments = 500;
        private Appointment[] _appointments = new Appointment[MaxAppointments];

        private PatientManager _patients;
        private DoctorManager _doctors;

        private int _count = 0;

        public int Count => _count;

        public Appointment? this[int index]
        {
            get
            {
                if (index < 0 || index >= _count)
                {
                    return null;
                }

                return _appointments[index];
            }
        }

        public AppointmentManager(PatientManager patients, DoctorManager doctors)
        {
            _patients = patients;
            _doctors = doctors;
        }

        private Appointment? FindById(int id)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_appointments[i].Id == id)
                {
                    return _appointments[i];
                }
            }

            return null;
        }

        public Appointment[] GetAll()
        {
            Appointment[] copy = new Appointment[_count];
            for (int i = 0; i < _count; i++)
            {
                copy[i] = _appointments[i];
            }
            return copy;
        }

        public static int CancelAll(ICancellable[] items, string reason = "")
        {
            int cancelledCount = 0;
            foreach (var item in items)
            {
                if (item.Cancel(reason))
                {
                    cancelledCount++;
                }
            }
            return cancelledCount;
        }

        private bool AddAppointment(int patientId, int doctorId, DateTime scheduledAt, Appointment app)
        {
            if (_count >= MaxAppointments)
            {
                Console.WriteLine("Error: record limit reached.");
                return false;
            }

            var patient = _patients.FindById(patientId);
            if (patient == null)
            {
                Console.WriteLine($"Error: patient with ID {patientId} not found.");
                return false;
            }

            var doctor = _doctors.FindById(doctorId);
            if (doctor == null)
            {
                Console.WriteLine($"Error: doctor with ID {doctorId} not found.");
                return false;
            }

            _appointments[_count] = app;
            _count++;

            Console.WriteLine($"Record [{app.Id}] created: {patient.FullName} > {doctor.FullName} о {scheduledAt:dd.MM.yyyy HH:mm}");
            return true;
        }

        public bool Book(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30)
        {
            var app = new RegularAppointment(patientId, doctorId, scheduledAt, durationMinutes);
            return AddAppointment(patientId, doctorId, scheduledAt, app);
        }

        public bool BookUrgent(int patientId, int doctorId, DateTime scheduledAt, string urgencyNote = "", int durationMinutes = 30)
        {
            var app = new UrgentAppointment(patientId, doctorId, scheduledAt, urgencyNote, durationMinutes);
            return AddAppointment(patientId, doctorId, scheduledAt, app);
        }

        public bool BookSpecialist(int patientId, int doctorId, DateTime scheduledAt, string topic = "", int durationMinutes = 45)
        {
            var app = new SpecialistAppointment(patientId, doctorId, scheduledAt, topic, durationMinutes);
            return AddAppointment(patientId, doctorId, scheduledAt, app);
        }

        public bool Cancel(int id, string reason = "")
        {
            var app = FindById(id);

            if (app == null)
            {
                Console.WriteLine($"Error: record with ID {id} not found.");
                return false;
            }

            bool result = app.Cancel(reason);

            if (result)
            {
                Console.WriteLine($"Record [{id}] has been cancelled.");
            }
            else
            {
                Console.WriteLine($"Failed to cancel record [{id}] (current status: {app.Status}).");
            }

            return result;
        }

        public bool Complete(int id)
        {
            var app = FindById(id);

            if (app == null)
            {
                Console.WriteLine($"Error: record with ID {id} not found.");
                return false;
            }

            bool result = app.Complete();

            if (result)
            {
                Console.WriteLine($"Record [{id}] completed.");
            }
            else
            {
                Console.WriteLine($"Failed to complete record [{id}] (current status {app.Status}).");
            }

            return result;
        }

        public Appointment[] GetByPatient(int patientId)
        {
            int matches = 0;

            for (int i = 0; i < _count; i++)
            {
                if (_appointments[i].PatientId == patientId)
                {
                    matches++;
                }
            }

            Appointment[] result = new Appointment[matches];
            int index = 0;

            for (int i = 0; i < _count; i++)
            {
                if (_appointments[i].PatientId == patientId)
                {
                    result[index++] = _appointments[i];
                }
            }

            return result;
        }

        public Appointment[] GetByDoctor(int doctorId)
        {
            int matches = 0;

            for (int i = 0; i < _count; i++)
            {
                if (_appointments[i].DoctorId == doctorId)
                {
                    matches++;
                }
            }

            Appointment[] result = new Appointment[matches];
            int index = 0;

            for (int i = 0; i < _count; i++)
            {
                if (_appointments[i].DoctorId == doctorId)
                {
                    result[index++] = _appointments[i];
                }
            }

            return result;
        }

        public Appointment[] GetByDate(DateTime date)
        {
            int matches = 0;

            for (int i = 0; i < _count; i++)
            {
                if (_appointments[i].ScheduledAt.Date == date.Date)
                {
                    matches++;
                }
            }

            Appointment[] result = new Appointment[matches];
            int index = 0;

            for (int i = 0; i < _count; i++)
            {
                if (_appointments[i].ScheduledAt.Date == date.Date)
                {
                    result[index++] = _appointments[i];
                }
            }

            return result;
        }

        public Appointment[] GetByDate(int year, int month, int day)
        {
            return GetByDate(new DateTime(year, month, day));
        }

        public Appointment[] GetUpcoming()
        {
            int matches = 0;

            for (int i = 0; i < _count; i++)
            {
                if (_appointments[i].IsUpcoming)
                {
                    matches++;
                }
            }

            Appointment[] result = new Appointment[matches];
            int index = 0;

            for (int i = 0; i < _count; i++)
            {
                if (_appointments[i].IsUpcoming)
                {
                    result[index++] = _appointments[i];
                }
            }

            return result;
        }

        // Методи фільтрації за типом для Задачі 4
        public Appointment[] GetUrgent()
        {
            int matches = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_appointments[i] is UrgentAppointment) matches++;
            }

            Appointment[] result = new Appointment[matches];
            int index = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_appointments[i] is UrgentAppointment)
                {
                    result[index++] = _appointments[i];
                }
            }
            return result;
        }

        public Appointment[] GetSpecialist()
        {
            int matches = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_appointments[i] is SpecialistAppointment) matches++;
            }

            Appointment[] result = new Appointment[matches];
            int index = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_appointments[i] is SpecialistAppointment)
                {
                    result[index++] = _appointments[i];
                }
            }
            return result;
        }

        public Appointment[] GetRegular()
        {
            int matches = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_appointments[i] is RegularAppointment) matches++;
            }

            Appointment[] result = new Appointment[matches];
            int index = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_appointments[i] is RegularAppointment)
                {
                    result[index++] = _appointments[i];
                }
            }
            return result;
        }

        public void DisplayAppointment(Appointment app)
        {
            var patient = _patients.FindById(app.PatientId);
            var doctor = _doctors.FindById(app.DoctorId);

            string patientName = patient != null ? patient.FullName : $"Patient #{app.PatientId}";
            string doctorName = doctor != null ? doctor.FullName : $"Doctor #{app.DoctorId}";

            string line = $"[{app.Id}] {app.GetDescription()} | {patientName} > {doctorName} | {app.ScheduledAt:dd.MM.yyyy HH:mm}–{app.EndsAt:HH:mm} | {app.Status}";

            if (app.Notes.Length > 0)
            {
                line += $" | {app.Notes}";
            }

            line += $" | {app.GetCost().ToString("F2")} грн";

            Console.WriteLine(line);
        }

        public void DisplayList(Appointment[] list)
        {
            if (list.Length == 0)
            {
                Console.WriteLine("No records found.");
                return;
            }

            foreach (var app in list)
            {
                DisplayAppointment(app);
            }
        }
    }
}
