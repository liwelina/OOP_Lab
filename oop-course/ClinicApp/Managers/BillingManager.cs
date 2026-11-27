using ClinicApp.Events;
using ClinicApp.Interfaces;
using ClinicApp.Models;

namespace ClinicApp.Managers
{
    public class BillingManager
    {
        private readonly AppointmentManager _appointments;

        public event EventHandler<PaymentEventArgs>? PaymentReceived;

        public BillingManager(AppointmentManager appointments)
        {
            _appointments = appointments;
        }

        private IPayable[] FilterUnpaid(Appointment[] sourceArray)
        {
            int matches = 0;
            for (int i = 0; i < sourceArray.Length; i++)
            {
                if (!sourceArray[i].IsPaid && !sourceArray[i].IsCancelled)
                {
                    matches++;
                }
            }

            IPayable[] result = new IPayable[matches];
            int index = 0;
            for (int i = 0; i < sourceArray.Length; i++)
            {
                if (!sourceArray[i].IsPaid && !sourceArray[i].IsCancelled)
                {
                    result[index++] = sourceArray[i];
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
            for (int i = 0; i < unpaid.Length; i++)
            {
                total += unpaid[i].GetCost();
            }
            return total;
        }

        public decimal GetPatientDebt(int patientId)
        {
            decimal total = 0m;
            IPayable[] unpaid = GetUnpaidByPatient(patientId);
            for (int i = 0; i < unpaid.Length; i++)
            {
                total += unpaid[i].GetCost();
            }
            return total;
        }

        public bool PayAppointment(int appointmentId)
        {
            Appointment[] all = _appointments.GetAll();
            for (int i = 0; i < all.Length; i++)
            {
                if (all[i].Id == appointmentId)
                {
                    if (!all[i].IsPaid && !all[i].IsCancelled)
                    {
                        all[i].MarkPaid();
                        PaymentReceived?.Invoke(this, new PaymentEventArgs(all[i].Id, all[i].GetCost()));
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

            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] is Appointment app)
                {
                    Console.Write(app.ToString() + " | ");
                }
                else
                {
                    Console.Write("Об'єкт | ");
                }

                Console.WriteLine($"Сума: {items[i].GetCost().ToString("F2")} грн");
            }
        }
    }
}