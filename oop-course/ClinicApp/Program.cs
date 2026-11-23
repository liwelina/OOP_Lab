using ClinicApp.Comparators;
using System.Text;
using ClinicApp;
using ClinicApp.Enums;
using ClinicApp.Interfaces;
using ClinicApp.Managers;
using ClinicApp.Models;
using ClinicApp.Utils;

namespace ClinicApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
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
            clinic.Logger.LogInfo($"Додано пацієнта #{patient1.Id}: {patient1.FullName}");
            clinic.Patients.Add(patient2);
            clinic.Logger.LogInfo($"Додано пацієнта #{patient2.Id}: {patient2.FullName}");
            clinic.Patients.Add(patient3);
            clinic.Logger.LogInfo($"Додано пацієнта #{patient3.Id}: {patient3.FullName}");

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
            clinic.Logger.LogInfo($"Додано лікаря #{doctor1.Id}: {doctor1.FullName}");
            clinic.Doctors.Add(doctor2);
            clinic.Logger.LogInfo($"Додано лікаря #{doctor2.Id}: {doctor2.FullName}");
            clinic.Doctors.Add(doctor3);
            clinic.Logger.LogInfo($"Додано лікаря #{doctor3.Id}: {doctor3.FullName}");

            bool booked1 = clinic.Appointments.Book(
                patient1.Id,
                doctor1.Id,
                DateTime.Now.AddDays(1).Date.AddHours(10));
            if (booked1)
            {
                clinic.Logger.LogInfo($"Створено запис: пацієнт #{patient1.Id} → лікар #{doctor1.Id}");
            }

            bool booked2 = clinic.Appointments.Book(
                patient2.Id,
                doctor2.Id,
                DateTime.Now.AddDays(2).Date.AddHours(12));
            if (booked2)
            {
                clinic.Logger.LogInfo($"Створено запис: пацієнт #{patient2.Id} → лікар #{doctor2.Id}");
            }

            bool booked3 = clinic.Appointments.Book(
                patient3.Id,
                doctor3.Id,
                DateTime.Now.AddDays(3).Date.AddHours(14));
            if (booked3)
            {
                clinic.Logger.LogInfo($"Створено запис: пацієнт #{patient3.Id} → лікар #{doctor3.Id}");
            }

            bool running = true;

            while (running)
            {
                Console.WriteLine();
                Console.WriteLine("╔══════════════════════════════════════════════╗");
                Console.WriteLine("║            МЕДИЧНА КЛІНІКА                   ║");
                Console.WriteLine("╠══════════════════════════════════════════════╣");
                Console.WriteLine("║  1. Пацієнти      — реєстрація, пошук        ║");
                Console.WriteLine("║  2. Лікарі        — персонал, розклад        ║");
                Console.WriteLine("║  3. Записи        — прийоми, скасування      ║");
                Console.WriteLine("║  4. Медична картка — діагнози, рецепти       ║");
                Console.WriteLine("║  5. Рахунки       — оплата, борги            ║");
                Console.WriteLine("║  6. Черга         — очікування, прийом       ║");
                Console.WriteLine("║  7. Звіт          — загальна статистика      ║");
                Console.WriteLine("║  8. Аналітика     — звіти та сортування      ║");
                Console.WriteLine("║  9. Плани лікування — рефлексія, атрибути    ║");
                Console.WriteLine("║ 10. Файли         — логи, робота з файлами   ║");
                Console.WriteLine("║  0. Вийти                                    ║");
                Console.WriteLine("╚══════════════════════════════════════════════╝");
                Console.Write("Оберіть розділ: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Невірний формат. Введіть число.");
                    clinic.Logger.LogWarning("Помилка введення: у головному меню введено не число.");
                    continue;
                }

                Console.WriteLine();

                switch (choice)
                {
                    case 1:
                        RunPatientsMenu(clinic);
                        break;

                    case 2:
                        RunDoctorsMenu(clinic);
                        break;

                    case 3:
                        RunAppointmentsMenu(clinic);
                        break;

                    case 4:
                        Console.WriteLine("Медична картка (функціонал попередніх лаб).");
                        break;

                    case 5:
                        RunBillingMenu(clinic);
                        break;

                    case 6:
                        WaitingRoomMenu(clinic);
                        break;

                    case 7:
                        clinic.GenerateReport();
                        break;

                    case 8:
                        AnalyticsMenu(clinic);
                        break;

                    case 9:
                        TreatmentPlansMenu(clinic);
                        break;

                    case 10:
                        FilesMenu(clinic);
                        break;

                    case 0:
                        running = false;
                        break;

                    default:
                        Console.WriteLine("Невідомий пункт меню.");
                        clinic.Logger.LogWarning($"Помилка введення: невідомий пункт головного меню ({choice}).");
                        break;
                }
            }

            Console.WriteLine("Program finished.");
        }

        private static void FilesMenu(Clinic clinic)
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine();
                Console.WriteLine("── Файли ───────────────────────");
                Console.WriteLine("1. Останні рядки лога");
                Console.WriteLine("2. Очистити лог");
                Console.WriteLine("3. Експортувати всі звіти");
                Console.WriteLine("4. Імпорт пацієнтів з CSV");
                Console.WriteLine("0. Назад");
                Console.Write("Оберіть: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Невірний формат. Введіть число.");
                    clinic.Logger.LogWarning("Помилка введення у меню «Файли»: введено не число.");
                    continue;
                }

                Console.WriteLine();

                switch (choice)
                {
                    case 1:
                        {
                            Console.Write("Скільки останніх рядків показати: ");
                            if (int.TryParse(Console.ReadLine(), out int n))
                            {
                                string[] lines = clinic.Logger.GetLastLines(n);
                                if (lines.Length == 0)
                                {
                                    Console.WriteLine("Лог порожній або файл відсутній.");
                                }
                                else
                                {
                                    foreach (string line in lines)
                                    {
                                        Console.WriteLine(line);
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Невірний формат кількості рядків.");
                                clinic.Logger.LogWarning("Помилка введення: некоректне число рядків лога.");
                            }
                            break;
                        }

                    case 2:
                        {
                            clinic.Logger.Clear();
                            Console.WriteLine("Лог очищено.");
                            break;
                        }

                    case 3:
                        {
                            string[] exportedPaths = clinic.Exporter.ExportAll();
                            Console.WriteLine("Експортовано звіти:");
                            foreach (string filePath in exportedPaths)
                            {
                                Console.WriteLine($"  - {filePath}");
                                clinic.Logger.LogInfo($"Експортовано звіт: {filePath}");
                            }
                            break;
                        }

                    case 4:
                        {
                            Console.Write("Шлях до CSV-файлу: ");
                            string path = Console.ReadLine()!.Trim();
                            ImportResult importResult = CsvImporter.ImportPatients(clinic, path);
                            importResult.Print();
                            clinic.Logger.LogInfo(
                                $"Імпорт з CSV '{path}': імпортовано {importResult.Imported}, пропущено {importResult.Skipped}.");
                            break;
                        }

                    case 0:
                        back = true;
                        break;

                    default:
                        Console.WriteLine("Невідомий пункт меню.");
                        break;
                }
            }
        }

        private static List<DoctorStats> CollectDoctorStats(Clinic clinic)
        {
            List<DoctorStats> list = new List<DoctorStats>();
            foreach (var stat in clinic.Analytics.ComputeDoctorStats())
            {
                list.Add(stat);
            }
            return list;
        }

        private static List<PatientStats> CollectPatientStats(Clinic clinic)
        {
            List<PatientStats> list = new List<PatientStats>();
            foreach (var stat in clinic.Analytics.ComputePatientStats())
            {
                list.Add(stat);
            }
            return list;
        }

        private static void AnalyticsMenu(Clinic clinic)
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine();
                Console.WriteLine("── Аналітика ───────────────────");
                Console.WriteLine("1. Лікарі за навантаженням");
                Console.WriteLine("2. Лікарі за виручкою");
                Console.WriteLine("3. Лікарі за іменем");
                Console.WriteLine("4. Пацієнти за кількістю візитів");
                Console.WriteLine("5. Пацієнти за витратами");
                Console.WriteLine("0. Назад");
                Console.Write("Оберіть: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Невірний формат. Введіть число.");
                    clinic.Logger.LogWarning("Помилка введення у меню «Аналітика»: введено не число.");
                    continue;
                }

                Console.WriteLine();

                switch (choice)
                {
                    case 1:
                        {
                            var doctors = CollectDoctorStats(clinic);
                            doctors.Sort();
                            Console.WriteLine("─── Лікарі за навантаженням ───");
                            foreach (var d in doctors)
                            {
                                Console.WriteLine(d);
                            }
                            break;
                        }
                    case 2:
                        {
                            var doctors = CollectDoctorStats(clinic);
                            doctors.Sort(new DoctorStatsByRevenue());
                            Console.WriteLine("─── Лікарі за виручкою ───");
                            foreach (var d in doctors)
                            {
                                Console.WriteLine(d);
                            }
                            break;
                        }
                    case 3:
                        {
                            var doctors = CollectDoctorStats(clinic);
                            doctors.Sort(new DoctorStatsByName());
                            Console.WriteLine("─── Лікарі за іменем ───");
                            foreach (var d in doctors)
                            {
                                Console.WriteLine(d);
                            }
                            break;
                        }
                    case 4:
                        {
                            var patients = CollectPatientStats(clinic);
                            patients.Sort();
                            Console.WriteLine("─── Пацієнти за кількістю візитів ───");
                            foreach (var p in patients)
                            {
                                Console.WriteLine(p);
                            }
                            break;
                        }
                    case 5:
                        {
                            var patients = CollectPatientStats(clinic);
                            patients.Sort(new PatientStatsBySpent());
                            Console.WriteLine("─── Пацієнти за витратами ───");
                            foreach (var p in patients)
                            {
                                Console.WriteLine(p);
                            }
                            break;
                        }
                    case 0:
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Невідомий пункт меню.");
                        break;
                }
            }
        }

        private static void WaitingRoomMenu(Clinic clinic)
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine();
                Console.WriteLine("── Черга ───────────────────────");
                Console.WriteLine("1. Додати пацієнта до черги");
                Console.WriteLine("2. Прийняти першого");
                Console.WriteLine("3. Хто перший?");
                Console.WriteLine("4. Переглянути всю чергу");
                Console.WriteLine("0. Назад");
                Console.Write("Оберіть: ");

                if (!int.TryParse(Console.ReadLine(), out int choice)) continue;
                Console.WriteLine();

                switch (choice)
                {
                    case 1:
                        Console.Write("Введіть ID пацієнта: ");
                        if (int.TryParse(Console.ReadLine(), out int patientId))
                        {
                            Patient? patient = clinic.Patients.FindById(patientId);
                            if (patient != null)
                            {
                                clinic.WaitingRoom.Enqueue(patient);
                                Console.WriteLine($"Пацієнта {patient.FullName} додано до черги.");
                                clinic.Logger.LogInfo($"Пацієнта #{patient.Id} ({patient.FullName}) додано до черги.");
                            }
                            else
                            {
                                Console.WriteLine("Пацієнта не знайдено.");
                                clinic.Logger.LogWarning($"Спроба додати до черги неіснуючого пацієнта #{patientId}.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Невірний формат ID.");
                            clinic.Logger.LogWarning("Помилка введення: невірний формат ID пацієнта в черзі.");
                        }
                        break;

                    case 2:
                        try
                        {
                            Patient accepted = clinic.WaitingRoom.Dequeue();
                            Console.WriteLine($"Прийнято: {accepted.FullName}. У черзі лишилось: {clinic.WaitingRoom.Count}");
                            clinic.Logger.LogInfo($"Прийнято пацієнта з черги: #{accepted.Id} ({accepted.FullName}).");
                        }
                        catch (InvalidOperationException ex)
                        {
                            Console.WriteLine($"Помилка: {ex.Message}");
                            clinic.Logger.LogWarning($"Помилка прийому з черги: {ex.Message}");
                        }
                        break;

                    case 3:
                        try
                        {
                            Patient first = clinic.WaitingRoom.Peek();
                            Console.WriteLine($"Перший у черзі: {first.FullName} (ID: {first.Id})");
                        }
                        catch (InvalidOperationException ex)
                        {
                            Console.WriteLine($"Помилка: {ex.Message}");
                            clinic.Logger.LogWarning($"Помилка перегляду першого в черзі: {ex.Message}");
                        }
                        break;

                    case 4:
                        Patient[] queueArray = clinic.WaitingRoom.ToArray();
                        if (queueArray.Length == 0)
                        {
                            Console.WriteLine("Черга порожня.");
                        }
                        else
                        {
                            Console.WriteLine("=== Поточна черга очікування ===");
                            for (int i = 0; i < queueArray.Length; i++)
                            {
                                Console.WriteLine($"{i + 1}. [{queueArray[i].Id}] {queueArray[i].FullName}");
                            }
                        }
                        break;

                    case 0:
                        back = true;
                        break;

                    default:
                        Console.WriteLine("Невідомий пункт меню.");
                        break;
                }
            }
        }

        private static void RunBillingMenu(Clinic clinic)
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine();
                Console.WriteLine("── Рахунки ───────────────────");
                Console.WriteLine("1. Борги пацієнта");
                Console.WriteLine("2. Всі неоплачені записи");
                Console.WriteLine("3. Оплатити запис");
                Console.WriteLine("4. Загальний борг клініки");
                Console.WriteLine("0. Назад");
                Console.Write("Оберіть: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Невірний формат. Введіть число.");
                    clinic.Logger.LogWarning("Помилка введення у меню «Рахунки»: введено не число.");
                    continue;
                }

                Console.WriteLine();

                switch (choice)
                {
                    case 1:
                        Console.Write("ID пацієнта: ");
                        if (int.TryParse(Console.ReadLine(), out int patientId))
                        {
                            IPayable[] unpaid = clinic.Billing.GetUnpaidByPatient(patientId);
                            decimal debt = clinic.Billing.GetPatientDebt(patientId);
                            Console.WriteLine($"Неоплачені записи пацієнта #{patientId}:");
                            clinic.Billing.DisplayUnpaid(unpaid);
                            Console.WriteLine($"Борг: {debt.ToString("F2")} грн");
                        }
                        else
                        {
                            Console.WriteLine("Невірний ID пацієнта.");
                            clinic.Logger.LogWarning("Помилка введення: невірний ID пацієнта при перевірці боргів.");
                        }
                        break;

                    case 2:
                        IPayable[] allUnpaid = clinic.Billing.GetAllUnpaid();
                        clinic.Billing.DisplayUnpaid(allUnpaid);
                        break;

                    case 3:
                        Console.Write("ID запису для оплати: ");
                        if (int.TryParse(Console.ReadLine(), out int appId))
                        {
                            bool paid = clinic.Billing.PayAppointment(appId);
                            if (paid)
                            {
                                Console.WriteLine($"Запис [{appId}] оплачено.");
                                clinic.Logger.LogInfo($"Оплачено запис #{appId}.");
                            }
                            else
                            {
                                Console.WriteLine("Не вдалося оплатити: запис не знайдено, вже оплачено або скасовано.");
                                clinic.Logger.LogWarning($"Неуспішна спроба оплати запису #{appId}.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Невірний ID запису.");
                            clinic.Logger.LogWarning("Помилка введення: невірний ID запису при оплаті.");
                        }
                        break;

                    case 4:
                        decimal totalDebt = clinic.Billing.GetTotalDebt();
                        Console.WriteLine($"Загальний борг по клініці: {totalDebt.ToString("F2")} грн");
                        break;

                    case 0:
                        back = true;
                        break;

                    default:
                        Console.WriteLine("Невідомий пункт меню.");
                        break;
                }
            }
        }

        private static void RunPatientsMenu(Clinic clinic)
        {
            clinic.Patients.DisplayAll();
        }

        private static void RunDoctorsMenu(Clinic clinic)
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine();
                Console.WriteLine("── Лікарі ────────────────────");
                Console.WriteLine("1. Показати всіх лікарів");
                Console.WriteLine("5. Вільні години лікаря");
                Console.WriteLine("0. Назад");
                Console.Write("Оберіть: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    continue;
                }

                Console.WriteLine();

                if (choice == 1)
                {
                    clinic.Doctors.DisplayAll();
                }
                else if (choice == 5)
                {
                    Console.Write("ID лікаря: ");
                    if (int.TryParse(Console.ReadLine(), out int docId))
                    {
                        Doctor? doc = clinic.Doctors.FindById(docId);
                        if (doc == null)
                        {
                            Console.WriteLine("Лікаря не знайдено.");
                            clinic.Logger.LogWarning($"Спроба переглянути слоти неіснуючого лікаря #{docId}.");
                            continue;
                        }

                        Console.Write("Дата (dd.MM.yyyy): ");
                        string dateStr = Console.ReadLine()!;
                        if (!DateTime.TryParseExact(dateStr, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime date))
                        {
                            Console.WriteLine("Невірний формат дати.");
                            clinic.Logger.LogWarning($"Помилка введення дати для лікаря #{docId}: '{dateStr}'.");
                            continue;
                        }

                        Console.Write("Скільки слотів: ");
                        if (!int.TryParse(Console.ReadLine(), out int slotsCount))
                        {
                            Console.WriteLine("Невірний формат кількості слотів.");
                            clinic.Logger.LogWarning("Помилка введення: невірний формат кількості слотів.");
                            continue;
                        }

                        try
                        {
                            ISchedulable schedulableDoctor = doc;
                            DateTime[] slots = schedulableDoctor.GetAvailableSlots(date, slotsCount);

                            foreach (var slot in slots)
                            {
                                Console.WriteLine(slot.ToString("HH:mm"));
                            }
                        }
                        catch (ArgumentOutOfRangeException ex)
                        {
                            Console.WriteLine($"Помилка: {ex.Message}");
                            clinic.Logger.LogWarning($"Помилка отримання слотів для лікаря #{docId}: {ex.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Невірний формат ID лікаря.");
                        clinic.Logger.LogWarning("Помилка введення: невірний формат ID лікаря.");
                    }
                }
                else if (choice == 0)
                {
                    back = true;
                }
            }
        }

        private static void RunAppointmentsMenu(Clinic clinic)
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine();
                Console.WriteLine("── Записи ────────────────────");
                Console.WriteLine("3. Майбутні записи");
                Console.WriteLine("8. Скасувати всі записи пацієнта");
                Console.WriteLine("0. Назад");
                Console.Write("Оберіть: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    continue;
                }

                Console.WriteLine();

                if (choice == 3)
                {
                    clinic.Appointments.DisplayList(clinic.Appointments.GetUpcoming());
                }
                else if (choice == 8)
                {
                    Console.Write("ID пацієнта: ");
                    if (int.TryParse(Console.ReadLine(), out int patientId))
                    {
                        Console.Write("Причина (Enter — без причини): ");
                        string reason = Console.ReadLine()!;

                        Appointment[] patientAppointments = clinic.Appointments.GetByPatient(patientId);
                        int cancelledCount = AppointmentManager.CancelAll(patientAppointments, reason);
                        Console.WriteLine($"Скасовано записів: {cancelledCount}");
                        clinic.Logger.LogInfo($"Скасовано записів для пацієнта #{patientId}: {cancelledCount} (причина: '{reason}').");
                    }
                    else
                    {
                        Console.WriteLine("Невірний ID пацієнта.");
                        clinic.Logger.LogWarning("Помилка введення: невірний ID пацієнта при скасуванні записів.");
                    }
                }
                else if (choice == 0)
                {
                    back = true;
                }
            }
        }

        private static void TreatmentPlansMenu(Clinic clinic)
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine();
                Console.WriteLine("── Плани лікування ────────────────────");
                Console.WriteLine("1. Показати всі плани");
                Console.WriteLine("2. Додати план лікування");
                Console.WriteLine("3. Плани пацієнта");
                Console.WriteLine("4. Активувати план");
                Console.WriteLine("5. Завершити план");
                Console.WriteLine("6. Скасувати план");
                Console.WriteLine("7. Інформація про тип TreatmentPlan");
                Console.WriteLine("0. Назад");
                Console.Write("Оберіть: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Невірний формат. Введіть число.");
                    clinic.Logger.LogWarning("Помилка введення у меню «Плани лікування»: введено не число.");
                    continue;
                }

                Console.WriteLine();

                switch (choice)
                {
                    case 1:
                        {
                            TreatmentPlan[] allPlans = clinic.TreatmentPlans.GetAll();
                            if (allPlans.Length == 0)
                            {
                                Console.WriteLine("Планів лікування немає.");
                            }
                            else
                            {
                                foreach (TreatmentPlan plan in allPlans)
                                {
                                    Console.WriteLine(plan);
                                }
                            }
                            break;
                        }

                    case 2:
                        {
                            TreatmentPlan newPlan = FormBuilder.Build<TreatmentPlan>();
                            bool added = clinic.TreatmentPlans.Add(newPlan);
                            if (added)
                            {
                                Console.WriteLine("План лікування додано.");
                                clinic.Logger.LogInfo($"Додано план лікування #{newPlan.Id} для пацієнта #{newPlan.PatientId}.");
                            }
                            else
                            {
                                clinic.Logger.LogWarning("Спроба додати невалідний план лікування.");
                            }
                            break;
                        }

                    case 3:
                        {
                            Console.Write("ID пацієнта: ");
                            if (int.TryParse(Console.ReadLine(), out int patientId))
                            {
                                TreatmentPlan[] patientPlans = clinic.TreatmentPlans.GetByPatient(patientId);
                                if (patientPlans.Length == 0)
                                {
                                    Console.WriteLine("Планів для цього пацієнта не знайдено.");
                                }
                                else
                                {
                                    foreach (TreatmentPlan plan in patientPlans)
                                    {
                                        Console.WriteLine(plan);
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Невірний ID пацієнта.");
                                clinic.Logger.LogWarning("Помилка введення: невірний ID пацієнта при пошуку планів.");
                            }
                            break;
                        }

                    case 4:
                        {
                            Console.Write("ID плану: ");
                            if (int.TryParse(Console.ReadLine(), out int planId))
                            {
                                TreatmentPlan? plan = clinic.TreatmentPlans.GetById(planId);
                                if (plan == null)
                                {
                                    Console.WriteLine("План не знайдено.");
                                    clinic.Logger.LogWarning($"Спроба активувати неіснуючий план #{planId}.");
                                }
                                else if (plan.Activate())
                                {
                                    Console.WriteLine("План активовано.");
                                    clinic.Logger.LogInfo($"Активовано план лікування #{plan.Id}.");
                                }
                                else
                                {
                                    Console.WriteLine("Неможливо активувати план у поточному статусі.");
                                    clinic.Logger.LogWarning($"Неможливо активувати план #{plan.Id} (статус: {plan.Status}).");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Невірний ID плану.");
                                clinic.Logger.LogWarning("Помилка введення: невірний ID плану при активації.");
                            }
                            break;
                        }

                    case 5:
                        {
                            Console.Write("ID плану: ");
                            if (int.TryParse(Console.ReadLine(), out int planId))
                            {
                                TreatmentPlan? plan = clinic.TreatmentPlans.GetById(planId);
                                if (plan == null)
                                {
                                    Console.WriteLine("План не знайдено.");
                                    clinic.Logger.LogWarning($"Спроба завершити неіснуючий план #{planId}.");
                                }
                                else if (plan.Complete())
                                {
                                    Console.WriteLine("План завершено.");
                                    clinic.Logger.LogInfo($"Завершено план лікування #{plan.Id}.");
                                }
                                else
                                {
                                    Console.WriteLine("Неможливо завершити план у поточному статусі.");
                                    clinic.Logger.LogWarning($"Неможливо завершити план #{plan.Id} (статус: {plan.Status}).");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Невірний ID плану.");
                                clinic.Logger.LogWarning("Помилка введення: невірний ID плану при завершенні.");
                            }
                            break;
                        }

                    case 6:
                        {
                            Console.Write("ID плану: ");
                            if (int.TryParse(Console.ReadLine(), out int planId))
                            {
                                TreatmentPlan? plan = clinic.TreatmentPlans.GetById(planId);
                                if (plan == null)
                                {
                                    Console.WriteLine("План не знайдено.");
                                    clinic.Logger.LogWarning($"Спроба скасувати неіснуючий план #{planId}.");
                                }
                                else if (plan.Cancel())
                                {
                                    Console.WriteLine("План скасовано.");
                                    clinic.Logger.LogInfo($"Скасовано план лікування #{plan.Id}.");
                                }
                                else
                                {
                                    Console.WriteLine("Неможливо скасувати план у поточному статусі.");
                                    clinic.Logger.LogWarning($"Неможливо скасувати план #{plan.Id} (статус: {plan.Status}).");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Невірний ID плану.");
                                clinic.Logger.LogWarning("Помилка введення: невірний ID плану при скасуванні.");
                            }
                            break;
                        }

                    case 7:
                        {
                            ModelValidator.PrintInfo(typeof(TreatmentPlan));
                            break;
                        }

                    case 0:
                        back = true;
                        break;

                    default:
                        Console.WriteLine("Невідомий пункт меню.");
                        break;
                }
            }
        }
    }
}