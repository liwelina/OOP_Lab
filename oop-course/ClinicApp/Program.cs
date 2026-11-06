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

            bool running = true;

            while (running)
            {
                Console.WriteLine();
                Console.WriteLine("╔══════════════════════════════════════════════╗");
                Console.WriteLine("║           МЕДИЧНА КЛІНІКА                    ║");
                Console.WriteLine("╠══════════════════════════════════════════════╣");
                Console.WriteLine("║  1. Пацієнти       — реєстрація, пошук       ║");
                Console.WriteLine("║  2. Лікарі         — персонал, розклад       ║");
                Console.WriteLine("║  3. Записи         — прийоми, скасування     ║");
                Console.WriteLine("║  4. Медична картка — діагнози, рецепти       ║");
                Console.WriteLine("║  5. Рахунки        — оплата, борги           ║");
                Console.WriteLine("║  6. Черга          — очікування, прийом      ║");
                Console.WriteLine("║  7. Звіт           — загальна статистика     ║");
                Console.WriteLine("║  0. Вийти                                    ║");
                Console.WriteLine("╚══════════════════════════════════════════════╝");
                Console.Write("Оберіть розділ: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Невірний формат. Введіть число.");
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
                        WaitingRoomMenu(clinic); // Пункт меню черги (Задача 3)
                        break;

                    case 7:
                        clinic.GenerateReport();
                        break;

                    case 0:
                        running = false;
                        break;

                    default:
                        Console.WriteLine("Невідомий пункт меню.");
                        break;
                }
            }

            Console.WriteLine("Program finished.");
        }

        // Меню черги очікування (Задача 3)
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
                            }
                            else
                            {
                                Console.WriteLine("Пацієнта не знайдено.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Невірний формат ID.");
                        }
                        break;

                    case 2:
                        try
                        {
                            Patient accepted = clinic.WaitingRoom.Dequeue();
                            Console.WriteLine($"Прийнято: {accepted.FullName}. У черзі лишилось: {clinic.WaitingRoom.Count}");
                        }
                        catch (InvalidOperationException ex)
                        {
                            Console.WriteLine($"Помилка: {ex.Message}");
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
                            }
                            else
                            {
                                Console.WriteLine("Не вдалося оплатити: запис не знайдено, вже оплачено або скасовано.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Невірний ID запису.");
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
                            continue;
                        }

                        Console.Write("Дата (dd.MM.yyyy): ");
                        string dateStr = Console.ReadLine()!;
                        if (!DateTime.TryParseExact(dateStr, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime date))
                        {
                            Console.WriteLine("Невірний формат дати.");
                            continue;
                        }

                        Console.Write("Скільки слотів: ");
                        if (!int.TryParse(Console.ReadLine(), out int slotsCount))
                        {
                            Console.WriteLine("Невірний формат кількості слотів.");
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
                        }
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
                    }
                    else
                    {
                        Console.WriteLine("Невірний ID пацієнта.");
                    }
                }
                else if (choice == 0)
                {
                    back = true;
                }
            }
        }
    }
}