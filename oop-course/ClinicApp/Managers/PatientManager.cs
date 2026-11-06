using ClinicApp.Enums;
using ClinicApp.Models;
using ClinicApp.Utils;

namespace ClinicApp.Managers
{
    public class PatientManager
    {
       
        private List<Patient> _patients = new List<Patient>();

        public int Count => _patients.Count;

    
        public Patient? this[int index]
        {
            get
            {
                if (index < 0 || index >= _patients.Count)
                {
                    return null;
                }
                return _patients[index];
            }
        }

        public void Add(Patient patient)
        {
            _patients.Add(patient);
            Console.WriteLine($"Patient registered: {patient.FullName} (ID: {patient.Id})");
        }

        public Patient? FindById(int id)
        {
            foreach (var patient in _patients)
            {
                if (patient.Id == id)
                {
                    return patient;
                }
            }
            return null;
        }

        public Patient[] FindByName(string firstName, string lastName)
        {
            List<Patient> matches = new List<Patient>();

            foreach (var patient in _patients)
            {
                if (patient.FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase) &&
                    patient.LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase))
                {
                    matches.Add(patient);
                }
            }

            return matches.ToArray();
        }

        public Patient[] FindByBloodType(BloodType bloodType)
        {
            List<Patient> matches = new List<Patient>();

            foreach (var patient in _patients)
            {
                if (patient.BloodType == bloodType)
                {
                    matches.Add(patient);
                }
            }

            return matches.ToArray();
        }

        public Patient[] GetAll()
        {
            
            return _patients.ToArray();
        }

        public bool Remove(int id)
        {
            var patient = FindById(id);
            if (patient == null)
            {
                Console.WriteLine($"Error: patient with ID {id} not found.");
                return false;
            }

            
            _patients.Remove(patient);
            Console.WriteLine($"Patient with ID {id} has been removed.");
            return true;
        }

        public void DisplayPatient(Patient patient)
        {
            Console.WriteLine($"[{patient.Id}] {patient.FullName} | DOB: {patient.BirthDate:dd.MM.yyyy} | Blood: {patient.BloodType} | Phone: {patient.PhoneNumber}");
        }

        public void DisplayAll()
        {
            if (_patients.Count == 0)
            {
                Console.WriteLine("No registered patients.");
                return;
            }

            Console.WriteLine("=== Patients List ===");
            foreach (var patient in _patients)
            {
                DisplayPatient(patient);
            }
        }
    }
}
