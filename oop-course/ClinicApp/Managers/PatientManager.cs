using ClinicApp.Events;
using ClinicApp.Models;

namespace ClinicApp.Managers
{
    public class PatientManager
    {
        private const int MaxPatients = 200;
        private Patient[] _patients = new Patient[MaxPatients];
        private int _count = 0;

        public int Count => _count;

        public event EventHandler<PatientEventArgs>? PatientAdded;

        public Patient? this[int index]
        {
            get
            {
                if (index < 0 || index >= _count)
                {
                    return null;
                }

                return _patients[index];
            }
        }

        public bool Add(Patient patient)
        {
            if (patient == null)
            {
                Console.WriteLine("Error: patient cannot be null.");
                return false;
            }

            if (_count >= MaxPatients)
            {
                Console.WriteLine("Error: patient limit reached.");
                return false;
            }

            _patients[_count] = patient;
            _count++;

            PatientAdded?.Invoke(this, new PatientEventArgs(patient.Id, patient.FullName));
            return true;
        }

        public Patient? FindById(int id)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_patients[i].Id == id)
                {
                    return _patients[i];
                }
            }

            return null;
        }

        public Patient[] GetAll()
        {
            Patient[] copy = new Patient[_count];
            for (int i = 0; i < _count; i++)
            {
                copy[i] = _patients[i];
            }
            return copy;
        }

        public void DisplayAll()
        {
            if (_count == 0)
            {
                Console.WriteLine("No patients registered.");
                return;
            }

            for (int i = 0; i < _count; i++)
            {
                Console.WriteLine(_patients[i]);
            }
        }
    }
}
