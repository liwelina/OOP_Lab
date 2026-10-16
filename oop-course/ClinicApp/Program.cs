using System;
using System.Globalization;
using System.Text;
using System.Threading;
using ClinicApp;
using ClinicApp.Enums;
using ClinicApp.Managers;
using ClinicApp.Models;
using ClinicApp.Utils;

namespace ClinicApp;

internal class Program
{
    static void Main(string[] args)
    {
        // Фіксуємо інваріантну культуру для коректного зчитування чисел з крапкою
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        Clinic clinic = new Clinic("City Clinic");

        Patient patient1 = new Patient(
            "Ivan",
            "Petrenko",
            new DateTime(1998, 5, 12),
            BloodType.APositive,
            "0501234567",
            "ivan@gmail.com");

        Patient patient2 = new Patient(
            "Olha",
            "Shevchenko",
            new DateTime(1985, 8, 20),
            BloodType.OPositive,
            "0672345678",
            "olha@gmail.com");

        Patient patient3 = new Patient(
            "Andrii",
            "Bondarenko",
            new DateTime(2010, 3, 15),
            BloodType.BPositive,
            "0933456789",
            "andrii@gmail.com");

        clinic.Patients.Add(patient1);
        clinic.Patients.Add(patient2);
        clinic.Patients.Add(patient3);

        Doctor doctor1 = new Doctor(
            "Anna",
            "Koval",
            Speciality.Cardiology,
            "LIC-001",
            "0501112233");

        Doctor doctor2 = new Doctor(
            "Petro",
            "Melnyk",
            Speciality.Neurology,
            "LIC-002",
            "0672223344");

        Doctor doctor3 = new Doctor(
            "Maria",
            "Tkachenko",
            Speciality.Pediatrics,
            "LIC-003",
            "0933334455");

        clinic.Doctors.Add(doctor1);
        clinic.Doctors.Add(doctor2);
        clinic.Doctors.Add(doctor3);

        clinic.Appointments.Book(
            patient1.Id,
            doctor1.Id,
            DateTime.Now.AddDays(1).Date.AddHours(10));

        clinic.Appointments.Book(
            patient2.Id,
            doctor2.Id,
            DateTime.Now.AddDays(2).Date.AddHours(12));

        clinic.Appointments.Book(
            patient3.Id,
            doctor3.Id,
            DateTime.Now.AddDays(3).Date.AddHours(14));

        // --- Тестові медичні записи ---
        clinic.MedicalRecords.Add(new Diagnosis(patient1.Id, doctor1.Id, DateTime.Today.AddDays(-30), "I10", "Гіпертонічна хвороба", isChronic: true));
        clinic.MedicalRecords.Add(new Diagnosis(patient1.Id, doctor1.Id, DateTime.Today.AddDays(-5), "J06.9", "Гострий ринофарингіт", isChronic: false));
        clinic.MedicalRecords.Add(new LabResult(patient1.Id, doctor1.Id, DateTime.Today.AddDays(-10), "Гемоглобін", 145, "г/л", "120–160", isNormal: true));
        clinic.MedicalRecords.Add(new LabResult(patient1.Id, doctor1.Id, DateTime.Today.AddDays(-10), "Холестерин", 6.2, "ммоль/л", "< 5.2", isNormal: false));
        clinic.MedicalRecords.Add(new Prescription(patient1.Id, doctor1.Id, DateTime.Today.AddDays(-5), "Лізиноприл", "10 мг", 30, "вранці"));

        // Рецепт, що закінчився (40 днів тому, курс 10 днів)
        clinic.MedicalRecords.Add(new Prescription(patient2.Id, doctor2.Id, DateTime.Today.AddDays(-40), "Парацетамол", "500 мг", 10, "при температурі"));

        // По одному запису для інших пацієнтів
        clinic.MedicalRecords.Add(new Diagnosis(patient2.Id, doctor2.Id, DateTime.Today.AddDays(-2), "G43", "Мігрень", isChronic: true));
        clinic.MedicalRecords.Add(new LabResult(patient3.Id, doctor3.Id, DateTime.Today.AddDays(-1), "Загальний аналіз крові", 5.0, "одиниць", "4.0–9.0", isNormal: true));

        bool running = true;

        while (running)
        {
            Console.WriteLine();
            Console.WriteLine("=== CLINIC MENU ===");
            Console.WriteLine("1. Show patients");
            Console.WriteLine("2. Show doctors");
            Console.WriteLine("3. Show appointments");
            Console.WriteLine("4. Медична картка");
            Console.WriteLine("5. Search doctor by speciality");
            Console.WriteLine("6. Add patient");
            Console.WriteLine("7. Add doctor");
            Console.WriteLine("8. Show statistics");
            Console.WriteLine("0. Exit");
            Console.Write("Choose: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Некоректний ввід. Введіть число.");
                continue;
            }

            Console.WriteLine();

            switch (choice)
            {
                case 1:
                    clinic.Patients.DisplayAll();
                    break;

                case 2:
                    clinic.Doctors.DisplayAll();
                    break;

                case 3:
                    clinic.Appointments.DisplayList(
                        clinic.Appointments.GetUpcoming());
                    break;

                case 4:
                    MedicalRecordsMenu(clinic);
                    break;

                case 5:
                    Console.WriteLine("=== Specialities ===");

                    int specialityNumber = 0;

                    foreach (Speciality speciality in Enum.GetValues<Speciality>())
                    {
                        Console.WriteLine($"{specialityNumber}. {speciality}");
                        specialityNumber++;
                    }

                    Console.Write("Choose speciality number: ");
                    if (!int.TryParse(Console.ReadLine(), out int selectedSpeciality))
                    {
                        Console.WriteLine("Некоректний номер спеціальності.");
                        break;
                    }

                    Speciality specialityValue = (Speciality)selectedSpeciality;

                    Doctor[] doctors = clinic.Doctors.FindBySpeciality(
                        specialityValue);

                    if (doctors.Length == 0)
                    {
                        Console.WriteLine("No doctors found.");
                    }
                    else
                    {
                        foreach (Doctor doctor in doctors)
                        {
                            Console.WriteLine(doctor);
                        }
                    }

                    break;

                case 6:
                    try
                    {
                        Console.Write("First name: ");
                        string firstName = Console.ReadLine()!;

                        Console.Write("Last name: ");
                        string lastName = Console.ReadLine()!;

                        Console.Write("Phone: ");
                        string phone = Console.ReadLine()!;

                        Console.WriteLine("=== Blood types ===");

                        int bloodTypeNumber = 0;

                        foreach (BloodType bloodType in Enum.GetValues<BloodType>())
                        {
                            Console.WriteLine($"{bloodTypeNumber}. {bloodType}");
                            bloodTypeNumber++;
                        }

                        Console.Write("Choose blood type number: ");
                        if (!int.TryParse(Console.ReadLine(), out int selectedBloodType))
                        {
                            Console.WriteLine("Некоректний номер групи крові.");
                            break;
                        }

                        BloodType bloodTypeValue = (BloodType)selectedBloodType;

                        Patient newPatient = new Patient(
                            firstName,
                            lastName,
                            DateTime.Today.AddYears(-25),
                            bloodTypeValue,
                            phone);

                        clinic.Patients.Add(newPatient);
                        Console.WriteLine("Patient added successfully!");
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        Console.WriteLine($"Помилка значення (діапазон): {ex.Message}");
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Помилка валідації: {ex.Message}");
                    }
                    break;

                case 7:
                    try
                    {
                        Console.Write("First name: ");
                        string doctorFirstName = Console.ReadLine()!;

                        Console.Write("Last name: ");
                        string doctorLastName = Console.ReadLine()!;

                        Console.Write("License number: ");
                        string licenseNumber = Console.ReadLine()!;

                        Console.Write("Phone: ");
                        string doctorPhone = Console.ReadLine()!;

                        Console.WriteLine("=== Specialities ===");

                        int doctorSpecialityNumber = 0;

                        foreach (Speciality speciality in Enum.GetValues<Speciality>())
                        {
                            Console.WriteLine($"{doctorSpecialityNumber}. {speciality}");
                            doctorSpecialityNumber++;
                        }

                        Console.Write("Choose speciality number: ");
                        if (!int.TryParse(Console.ReadLine(), out int doctorSelectedSpeciality))
                        {
                            Console.WriteLine("Некоректний номер спеціальності.");
                            break;
                        }

                        Speciality doctorSpecialityValue =
                            (Speciality)doctorSelectedSpeciality;

                        Doctor newDoctor = new Doctor(
                            doctorFirstName,
                            doctorLastName,
                            doctorSpecialityValue,
                            licenseNumber,
                            doctorPhone);

                        clinic.Doctors.Add(newDoctor);
                        Console.WriteLine("Doctor added successfully!");
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        Console.WriteLine($"Помилка значення (діапазон): {ex.Message}");
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Помилка валідації: {ex.Message}");
                    }
                    break;

                case 8:
                    clinic.Patients.DisplayStats();
                    Console.WriteLine();
                    clinic.Doctors.DisplayStats();
                    break;

                case 0:
                    running = false;
                    break;

                default:
                    Console.WriteLine("Unknown menu option.");
                    break;
            }
        }

        Console.WriteLine("Program finished.");
    }

    private static void MedicalRecordsMenu(Clinic clinic)
    {
        bool inSubMenu = true;

        while (inSubMenu)
        {
            Console.WriteLine();
            Console.WriteLine("── Медична картка ────────────");
            Console.WriteLine("1. Картка пацієнта (зведення)");
            Console.WriteLine("2. Усі записи пацієнта (поліморфний вивід)");
            Console.WriteLine("3. Додати діагноз");
            Console.WriteLine("4. Додати аналіз");
            Console.WriteLine("5. Додати рецепт");
            Console.WriteLine("6. Записи лікаря");
            Console.WriteLine("0. Назад");
            Console.Write("Оберіть: ");

            if (!int.TryParse(Console.ReadLine(), out int subChoice))
            {
                Console.WriteLine("Некоректний ввід.");
                continue;
            }

            Console.WriteLine();

            switch (subChoice)
            {
                case 1:
                    clinic.Patients.DisplayAll();
                    Console.Write("ID пацієнта: ");
                    if (!int.TryParse(Console.ReadLine(), out int pId1))
                    {
                        Console.WriteLine("Некоректне число.");
                        break;
                    }
                    if (clinic.Patients.FindById(pId1) == null)
                    {
                        Console.WriteLine("Пацієнта не знайдено.");
                        break;
                    }
                    clinic.MedicalRecords.DisplayPatientSummary(pId1);
                    break;

                case 2:
                    clinic.Patients.DisplayAll();
                    Console.Write("ID пацієнта: ");
                    if (!int.TryParse(Console.ReadLine(), out int pId2))
                    {
                        Console.WriteLine("Некоректне число.");
                        break;
                    }
                    if (clinic.Patients.FindById(pId2) == null)
                    {
                        Console.WriteLine("Пацієнта не знайдено.");
                        break;
                    }
                    MedicalRecord[] records = clinic.MedicalRecords.GetByPatient(pId2);
                    clinic.MedicalRecords.DisplayList(records);
                    break;

                case 3:
                    try
                    {
                        clinic.Patients.DisplayAll();
                        Console.Write("ID пацієнта: ");
                        if (!int.TryParse(Console.ReadLine(), out int patId)) { Console.WriteLine("Некоректне число."); break; }

                        clinic.Doctors.DisplayAll();
                        Console.Write("ID лікаря: ");
                        if (!int.TryParse(Console.ReadLine(), out int docId)) { Console.WriteLine("Некоректне число."); break; }

                        if (clinic.Patients.FindById(patId) == null || clinic.Doctors.FindById(docId) == null)
                        {
                            Console.WriteLine("Пацієнта або лікаря не знайдено.");
                            break;
                        }

                        Console.Write("Код діагнозу (напр. J06.9): ");
                        string code = Console.ReadLine()!;

                        Console.Write("Опис: ");
                        string desc = Console.ReadLine()!;

                        Console.Write("Хронічне? (1=так, 0=ні): ");
                        if (!int.TryParse(Console.ReadLine(), out int chronicInput)) { Console.WriteLine("Некоректне число."); break; }
                        bool isChronic = chronicInput == 1;

                        Diagnosis diag = new Diagnosis(patId, docId, DateTime.Today, code, desc, isChronic);
                        clinic.MedicalRecords.Add(diag);
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        Console.WriteLine($"Помилка: {ex.Message}");
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Помилка: {ex.Message}");
                    }
                    break;

                case 4:
                    try
                    {
                        clinic.Patients.DisplayAll();
                        Console.Write("ID пацієнта: ");
                        if (!int.TryParse(Console.ReadLine(), out int patId)) { Console.WriteLine("Некоректне число."); break; }

                        clinic.Doctors.DisplayAll();
                        Console.Write("ID лікаря: ");
                        if (!int.TryParse(Console.ReadLine(), out int docId)) { Console.WriteLine("Некоректне число."); break; }

                        if (clinic.Patients.FindById(patId) == null || clinic.Doctors.FindById(docId) == null)
                        {
                            Console.WriteLine("Пацієнта або лікаря не знайдено.");
                            break;
                        }

                        Console.Write("Назва аналізу: ");
                        string testName = Console.ReadLine()!;

                        Console.Write("Значення (число): ");
                        string valStr = Console.ReadLine()!;
                        if (!double.TryParse(valStr, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                        {
                            Console.WriteLine("Некоректне числове значення аналізу.");
                            break;
                        }

                        Console.Write("Одиниці виміру: ");
                        string unit = Console.ReadLine()!;

                        Console.Write("Норма (напр. 4.0–9.0): ");
                        string range = Console.ReadLine()!;

                        Console.Write("В нормі? (1=так, 0=ні): ");
                        if (!int.TryParse(Console.ReadLine(), out int normInput)) { Console.WriteLine("Некоректне число."); break; }
                        bool isNormal = normInput == 1;

                        LabResult lab = new LabResult(patId, docId, DateTime.Today, testName, value, unit, range, isNormal);
                        clinic.MedicalRecords.Add(lab);
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        Console.WriteLine($"Помилка: {ex.Message}");
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Помилка: {ex.Message}");
                    }
                    break;

                case 5:
                    try
                    {
                        clinic.Patients.DisplayAll();
                        Console.Write("ID пацієнта: ");
                        if (!int.TryParse(Console.ReadLine(), out int patId)) { Console.WriteLine("Некоректне число."); break; }

                        clinic.Doctors.DisplayAll();
                        Console.Write("ID лікаря: ");
                        if (!int.TryParse(Console.ReadLine(), out int docId)) { Console.WriteLine("Некоректне число."); break; }

                        if (clinic.Patients.FindById(patId) == null || clinic.Doctors.FindById(docId) == null)
                        {
                            Console.WriteLine("Пацієнта або лікаря не знайдено.");
                            break;
                        }

                        Console.Write("Препарат: ");
                        string med = Console.ReadLine()!;

                        Console.Write("Дозування (напр. 10 мг): ");
                        string dosage = Console.ReadLine()!;

                        Console.Write("Кількість днів (ціле): ");
                        if (!int.TryParse(Console.ReadLine(), out int days))
                        {
                            Console.WriteLine("Некоректне число днів.");
                            break;
                        }

                        Console.Write("Інструкція (Enter — пропустити): ");
                        string instructions = Console.ReadLine()!;

                        Prescription rx = new Prescription(patId, docId, DateTime.Today, med, dosage, days, instructions);
                        clinic.MedicalRecords.Add(rx);
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        Console.WriteLine($"Помилка: {ex.Message}");
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Помилка: {ex.Message}");
                    }
                    break;

                case 6:
                    clinic.Doctors.DisplayAll();
                    Console.Write("ID лікаря: ");
                    if (!int.TryParse(Console.ReadLine(), out int dId))
                    {
                        Console.WriteLine("Некоректне число.");
                        break;
                    }
                    if (clinic.Doctors.FindById(dId) == null)
                    {
                        Console.WriteLine("Лікаря не знайдено.");
                        break;
                    }
                    MedicalRecord[] docRecords = clinic.MedicalRecords.GetByDoctor(dId);
                    clinic.MedicalRecords.DisplayList(docRecords);
                    break;

                case 0:
                    inSubMenu = false;
                    break;

                default:
                    Console.WriteLine("Невідомий пункт меню.");
                    break;
            }
        }
    }
}