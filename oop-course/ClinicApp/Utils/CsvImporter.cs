using System.Globalization;
using System.Text;
using ClinicApp.Enums;
using ClinicApp.Models;

namespace ClinicApp.Utils
{
    public static class CsvImporter
    {
        public static ImportResult ImportPatients(Clinic clinic, string filePath)
        {
            ImportResult result = new ImportResult();

            if (!File.Exists(filePath))
            {
                result.AddError(0, $"Файл не знайдено ({filePath}).");
                return result;
            }

            string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);

            for (int i = 0; i < lines.Length; i++)
            {
                int lineNumber = i + 1;
                string line = lines[i];

                // Перший рядок — заголовок CSV
                if (i == 0)
                {
                    continue;
                }

                // Порожні рядки пропускаємо
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                try
                {
                    string[] parts = line.Split(',');
                    if (parts.Length != 5)
                    {
                        result.AddError(lineNumber, $"Рядок має {parts.Length} полів замість 5.");
                        continue;
                    }

                    string firstName = parts[0].Trim();
                    string lastName = parts[1].Trim();
                    string birthDateStr = parts[2].Trim();
                    string bloodTypeStr = parts[3].Trim();
                    string phone = parts[4].Trim();

                    if (string.IsNullOrWhiteSpace(firstName))
                    {
                        result.AddError(lineNumber, "Ім'я не може бути порожнім.");
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(lastName))
                    {
                        result.AddError(lineNumber, "Прізвище не може бути порожнім.");
                        continue;
                    }

                    DateTime dateOfBirth = DateTime.ParseExact(
                        birthDateStr,
                        "dd.MM.yyyy",
                        CultureInfo.InvariantCulture);

                    BloodType bloodType = (BloodType)Enum.Parse(typeof(BloodType), bloodTypeStr, true);

                    string defaultEmail = $"{firstName.ToLower()}.{lastName.ToLower()}@clinic.local";
                    Patient patient = new Patient(
                        firstName,
                        lastName,
                        dateOfBirth,
                        bloodType,
                        phone,
                        defaultEmail);

                    clinic.Patients.Add(patient);
                    result.AddSuccess();
                }
                catch (FormatException)
                {
                    result.AddError(lineNumber, "Невірний формат дати (очікується dd.MM.yyyy).");
                }
                catch (ArgumentException ex)
                {
                    result.AddError(lineNumber, ex.Message);
                }
                catch (Exception ex)
                {
                    result.AddError(lineNumber, ex.Message);
                }
            }

            return result;
        }
    }
}