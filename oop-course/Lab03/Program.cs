using System.Text;

namespace Lab03;

internal class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        PatientManager patientManager = new PatientManager();
        DoctorManager doctorManager = new DoctorManager();
        AppointmentManager appointmentManager = new AppointmentManager(patientManager, doctorManager);

        patientManager.Add(new Patient("Ivan", "Petrenko", DateTime.Today.AddYears(-41), "A+", "0501234567"));
        patientManager.Add(new Patient("Olena", "Koval", DateTime.Today.AddYears(-33), "B-", "0672345678"));
        patientManager.Add(new Patient("Maksym", "Boyko", DateTime.Today.AddYears(-16), "O+", "0933456789"));
        patientManager.Add(new Patient("Maria", "Tkach"));

        doctorManager.Add(new Doctor("Oleh", "Sydorenko", "Cardiology", "LIC-001", "0441234567")
        {
            WorkStartHour = 8,
            WorkEndHour = 16
        });
        doctorManager.Add(new Doctor("Nataliia", "Moroz", "Neurology", "LIC-002", "0442345678")
        {
            WorkStartHour = 9,
            WorkEndHour = 18
        });
        doctorManager.Add(new Doctor("Andrii", "Vlasenko", "Pediatrics", "LIC-003", "0443456789"));

        appointmentManager.Book(1, 1, DateTime.Now.AddDays(1).Date.AddHours(10), 30);
        appointmentManager.Book(2, 2, DateTime.Now.AddDays(1).Date.AddHours(11), 45);
        appointmentManager.Book(3, 3, DateTime.Now.AddDays(2).Date.AddHours(9), 20);

        while (true)
        {
            Console.WriteLine("\n=== CLINIC SYSTEM ===");
            Console.WriteLine("1. Patients Menu");
            Console.WriteLine("2. Doctors Menu");
            Console.WriteLine("3. Appointments Menu");
            Console.WriteLine("0. Exit");
            Console.Write("Select an option: ");

            string? choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    RunPatientMenu(patientManager);
                    break;
                case "2":
                    RunDoctorMenu(doctorManager);
                    break;
                case "3":
                    RunAppointmentMenu(appointmentManager, patientManager, doctorManager);
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    private static void RunPatientMenu(PatientManager manager)
    {
        while (true)
        {
            Console.WriteLine("\n--- Menu: Patients ---");
            Console.WriteLine("1. View all");
            Console.WriteLine("2. Add patient");
            Console.WriteLine("3. Find by name");
            Console.WriteLine("4. Delete patient");
            Console.WriteLine("5. Statistics");
            Console.WriteLine("0. Back to main menu");
            Console.Write("Select an action: ");

            string? choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    manager.DisplayAll();
                    break;
                case "2":
                    Console.Write("First name: ");
                    string fn = Console.ReadLine() ?? "";
                    Console.Write("Last name: ");
                    string ln = Console.ReadLine() ?? "";
                    Console.Write("Age: ");
                    int.TryParse(Console.ReadLine(), out int age);
                    Console.Write("Blood type: ");
                    string blood = Console.ReadLine() ?? "Unknown";
                    Console.Write("Phone: ");
                    string phone = Console.ReadLine() ?? "0000000000";
                    manager.Add(new Patient(fn, ln, DateTime.Today.AddYears(-age), blood, phone));
                    break;
                case "3":
                    Console.Write("Enter first or last name: ");
                    string q = Console.ReadLine() ?? "";
                    var found = manager.FindByName(q);
                    if (found.Length == 0) Console.WriteLine("No one found.");
                    else foreach (var p in found) Console.WriteLine(p);
                    break;
                case "4":
                    Console.Write("Patient ID to delete: ");
                    if (int.TryParse(Console.ReadLine(), out int id))
                    {
                        if (manager.Remove(id)) Console.WriteLine($"Patient #{id} removed.");
                        else Console.WriteLine($"Patient #{id} not found.");
                    }
                    break;
                case "5":
                    manager.DisplayStats();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }

    private static void RunDoctorMenu(DoctorManager manager)
    {
        while (true)
        {
            Console.WriteLine("\n--- Menu: Doctors ---");
            Console.WriteLine("1. View all");
            Console.WriteLine("2. Add doctor");
            Console.WriteLine("3. Find by speciality");
            Console.WriteLine("4. Delete doctor");
            Console.WriteLine("5. Statistics");
            Console.WriteLine("0. Back to main menu");
            Console.Write("Select an action: ");

            string? choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    manager.DisplayAll();
                    break;
                case "2":
                    Console.Write("First name: ");
                    string fn = Console.ReadLine() ?? "";
                    Console.Write("Last name: ");
                    string ln = Console.ReadLine() ?? "";
                    Console.Write("Speciality: ");
                    string sp = Console.ReadLine() ?? "";
                    Console.Write("License: ");
                    string lic = Console.ReadLine() ?? "";
                    Console.Write("Phone: ");
                    string ph = Console.ReadLine() ?? "";
                    Console.Write("Shift start hour (0-23): ");
                    int.TryParse(Console.ReadLine(), out int start);
                    Console.Write("Shift end hour (0-23): ");
                    int.TryParse(Console.ReadLine(), out int end);
                    manager.Add(new Doctor(fn, ln, sp, lic, ph) { WorkStartHour = start, WorkEndHour = end });
                    break;
                case "3":
                    Console.Write("Speciality to search: ");
                    string sQuery = Console.ReadLine() ?? "";
                    var docs = manager.FindBySpeciality(sQuery);
                    if (docs.Length == 0) Console.WriteLine("No doctors found.");
                    else foreach (var d in docs) Console.WriteLine(d);
                    break;
                case "4":
                    Console.Write("Doctor ID to delete: ");
                    if (int.TryParse(Console.ReadLine(), out int id))
                    {
                        if (manager.Remove(id)) Console.WriteLine($"Doctor #{id} removed.");
                        else Console.WriteLine($"Doctor #{id} not found.");
                    }
                    break;
                case "5":
                    manager.DisplayStats();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }

    private static void RunAppointmentMenu(AppointmentManager appManager, PatientManager pManager, DoctorManager dManager)
    {
        while (true)
        {
            Console.WriteLine("\n--- Menu: Appointments ---");
            Console.WriteLine("1. Upcoming appointments");
            Console.WriteLine("2. Book appointment");
            Console.WriteLine("3. Cancel appointment");
            Console.WriteLine("4. Complete appointment");
            Console.WriteLine("5. All appointments of patient");
            Console.WriteLine("6. All appointments of doctor");
            Console.WriteLine("7. Appointments by date");
            Console.WriteLine("0. Back to main menu");
            Console.Write("Select an action: ");

            string? choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("Upcoming appointments:");
                    appManager.DisplayList(appManager.GetUpcoming());
                    break;

                case "2":
                    Console.WriteLine("--- Available patients ---");
                    pManager.DisplayAll();
                    Console.Write("Enter patient ID: ");
                    int.TryParse(Console.ReadLine(), out int pId);

                    Console.WriteLine("--- Available doctors ---");
                    dManager.DisplayAll();
                    Console.Write("Enter doctor ID: ");
                    int.TryParse(Console.ReadLine(), out int dId);

                    Console.Write("Date and time (yyyy-MM-dd HH:mm): ");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime dt))
                    {
                        dt = DateTime.Now.AddHours(2);
                    }

                    Console.Write("Duration in minutes (default 30): ");
                    if (!int.TryParse(Console.ReadLine(), out int dur) || dur <= 0)
                    {
                        dur = 30;
                    }

                    appManager.Book(pId, dId, dt, dur);
                    break;

                case "3":
                    Console.Write("Enter appointment ID to cancel: ");
                    int.TryParse(Console.ReadLine(), out int cancelId);
                    Console.Write("Reason: ");
                    string reason = Console.ReadLine() ?? "";
                    appManager.Cancel(cancelId, reason);
                    break;

                case "4":
                    Console.Write("Enter appointment ID to complete: ");
                    int.TryParse(Console.ReadLine(), out int compId);
                    appManager.Complete(compId);
                    break;

                case "5":
                    Console.Write("Enter patient ID: ");
                    int.TryParse(Console.ReadLine(), out int searchPId);
                    appManager.DisplayList(appManager.GetByPatient(searchPId));
                    break;

                case "6":
                    Console.Write("Enter doctor ID: ");
                    int.TryParse(Console.ReadLine(), out int searchDId);
                    appManager.DisplayList(appManager.GetByDoctor(searchDId));
                    break;

                case "7":
                    Console.Write("Enter date (yyyy-MM-dd): ");
                    if (DateTime.TryParse(Console.ReadLine(), out DateTime searchDate))
                    {
                        appManager.DisplayList(appManager.GetByDate(searchDate));
                    }
                    else
                    {
                        Console.WriteLine("Invalid date format.");
                    }
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }
}