using System.Globalization;
using System.Text;
using ClinicApp.Enums;
using ClinicApp.Models;

namespace ClinicApp.Utils
{
    public class SessionManager
    {
        private readonly string _sessionPath;

        public SessionManager(string sessionPath = "session.dat")
        {
            _sessionPath = sessionPath;
        }

        public bool Exists()
        {
            return File.Exists(_sessionPath);
        }

        public void Save(Clinic clinic)
        {
            Patient[] patients = clinic.Patients.GetAll();

            using (StreamWriter writer = new StreamWriter(_sessionPath, false, Encoding.UTF8))
            {
                writer.WriteLine("[PATIENTS]");

                for (int i = 0; i < patients.Length; i++)
                {
                    Patient p = patients[i];
                    writer.WriteLine($"{p.FirstName},{p.LastName},{p.DateOfBirth:dd.MM.yyyy},{p.BloodType},{p.Phone}");
                }

                writer.WriteLine("[END]");
            }
        }

        public int Load(Clinic clinic)
        {
            if (!Exists())
            {
                return 0;
            }

            string[] lines = File.ReadAllLines(_sessionPath, Encoding.UTF8);
            string currentSection = "";
            int addedCount = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();

                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    currentSection = line;
                    continue;
                }

                if (currentSection != "[PATIENTS]")
                {
                    continue;
                }

                try
                {
                    string[] parts = line.Split(',');
                    if (parts.Length != 5)
                    {
                        continue;
                    }

                    string firstName = parts[0].Trim();
                    string lastName = parts[1].Trim();
                    string birthDateStr = parts[2].Trim();
                    string bloodTypeStr = parts[3].Trim();
                    string phone = parts[4].Trim();

                    if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
                    {
                        continue;
                    }

                    DateTime dateOfBirth = DateTime.ParseExact(
                        birthDateStr,
                        "dd.MM.yyyy",
                        CultureInfo.InvariantCulture);

                    BloodType bloodType = (BloodType)Enum.Parse(typeof(BloodType), bloodTypeStr, true);

                    if (IsDuplicate(clinic, firstName, lastName, dateOfBirth))
                    {
                        continue;
                    }

                    string defaultEmail = $"{firstName.ToLower()}.{lastName.ToLower()}@clinic.local";
                    Patient patient = new Patient(
                        firstName,
                        lastName,
                        dateOfBirth,
                        bloodType,
                        phone,
                        defaultEmail);

                    clinic.Patients.Add(patient);
                    addedCount++;
                }
                catch
                {

                }
            }

            return addedCount;
        }

        private static bool IsDuplicate(Clinic clinic, string firstName, string lastName, DateTime dateOfBirth)
        {
            Patient[] existing = clinic.Patients.GetAll();
            for (int i = 0; i < existing.Length; i++)
            {
                if (string.Equals(existing[i].FirstName, firstName, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(existing[i].LastName, lastName, StringComparison.OrdinalIgnoreCase) &&
                    existing[i].DateOfBirth.Date == dateOfBirth.Date)
                {
                    return true;
                }
            }
            return false;
        }
    }
}