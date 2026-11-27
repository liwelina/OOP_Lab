using ClinicApp.Events;
using ClinicApp.Models;

namespace ClinicApp.Managers
{
    public class TreatmentPlanManager
    {
        private const int MaxPlans = 200;
        private TreatmentPlan[] _plans = new TreatmentPlan[MaxPlans];
        private int _count = 0;

        public int Count => _count;

        public event EventHandler<TreatmentPlanEventArgs>? PlanCompleted;

        public bool Add(TreatmentPlan plan)
        {
            if (plan == null)
            {
                Console.WriteLine("Помилка: план лікування не може бути порожнім.");
                return false;
            }

            if (_count >= MaxPlans)
            {
                Console.WriteLine("Помилка: досягнуто ліміт планів лікування.");
                return false;
            }

            _plans[_count] = plan;
            _count++;
            return true;
        }

        public TreatmentPlan? GetById(int id)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_plans[i].Id == id)
                {
                    return _plans[i];
                }
            }

            return null;
        }

        public TreatmentPlan[] GetAll()
        {
            TreatmentPlan[] copy = new TreatmentPlan[_count];
            for (int i = 0; i < _count; i++)
            {
                copy[i] = _plans[i];
            }
            return copy;
        }

        public TreatmentPlan[] GetByPatient(int patientId)
        {
            int matches = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_plans[i].PatientId == patientId)
                {
                    matches++;
                }
            }

            TreatmentPlan[] result = new TreatmentPlan[matches];
            int index = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_plans[i].PatientId == patientId)
                {
                    result[index++] = _plans[i];
                }
            }

            return result;
        }

        public bool Complete(int planId)
        {
            TreatmentPlan? plan = GetById(planId);
            if (plan == null)
            {
                return false;
            }

            if (!plan.Complete())
            {
                return false;
            }

            PlanCompleted?.Invoke(this, new TreatmentPlanEventArgs(plan.Id, plan.PatientId, plan.Diagnosis));
            return true;
        }
    }
}