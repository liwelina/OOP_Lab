using ClinicApp.Interfaces;
using ClinicApp.Models;

namespace ClinicApp.Managers
{
    public class BillingManager
    {
        private readonly AppointmentManager _appointments;

        public BillingManager(AppointmentManager appointments)
        {
            _appointments = appointments;
        }

        
        private IPayable[] FilterUnpaid(Appointment[] sourceArray)
        {
            int matches = 0;
            foreach (var app in sourceArray)
            {
                if (!app.IsPaid && !app.IsCancelled)
                {
                    matches++;
                }
            }

            IPayable[] result = new IPayable[matches];
            int index = 0;
            foreach (var app in sourceArray)
            {
                if (!app.IsPaid && !app.IsCancelled)
                {
                    result[index++] = app;
                }
            }

            return result;
        }

        public IPayable[] GetAllUnpaid()
        {
            return FilterUnpaid(_appointments.GetAll());
        }

        public IPayable[] GetUnpaidByPatient(int patientId)
        {
            return FilterUnpaid(_appointments.GetByPatient(patientId));
        }

        public decimal GetTotalDebt()
        {
            decimal total = 0m;
            IPayable[] unpaid = GetAllUnpaid();
            foreach (var item in unpaid)
            {
                total += item.GetCost();
            }
            return total;
        }

        public decimal GetPatientDebt(int patientId)
        {
            decimal total = 0m;
            IPayable[] unpaid = GetUnpaidByPatient(patientId);
            foreach (var item in unpaid)
            {
                total += item.GetCost();
            }
            return total;
        }

        public bool PayAppointment(int appointmentId)
        {
            Appointment[] all = _appointments.GetAll();
            foreach (var app in all)
            {
                if (app.Id == appointmentId)
                {
                    
                    if (!app.IsPaid && !app.IsCancelled)
                    {
                        app.MarkPaid();
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }

        public void DisplayUnpaid(IPayable[] items)
        {
            if (items.Length == 0)
            {
                Console.WriteLine("Немає неоплачених записів.");
                return;
            }

            foreach (var item in items)
            {
               
                if (item is Appointment app)
                {
                    Console.Write(app.ToString() + " | ");
                }
                else
                {
                    Console.Write("Об'єкт | ");
                }

                // Виведення суми у форматі F2 з гривнями
                Console.WriteLine($"Сума: {item.GetCost().ToString("F2")} грн");
            }
        }
    }
}
