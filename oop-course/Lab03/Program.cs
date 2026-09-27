using System.Text;

namespace Lab03;

internal class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        Clinic clinic = new Clinic("Medical Clinic");

        clinic.Patients.Add(new Patient("Ivan", "Petrenko", DateTime.Today.AddYears(-41), "A+", "0501234567"));
        clinic.Patients.Add(new Patient("Olena", "Koval", DateTime.Today.AddYears(-33), "B-", "0672345678"));
        clinic.Patients.Add(new Patient("Maksym", "Boyko", DateTime.Today.AddYears(-16), "O+", "0933456789"));
        clinic.Patients.Add(new Patient("Maria", "Tkach"));

        clinic.Doctors.Add(new Doctor("Oleh", "Sydorenko", "Cardiology", "LIC-001", "0441234567")
        {
            WorkStartHour = 8,
            WorkEndHour = 16
        });
        clinic.Doctors.Add(new Doctor("Nataliia", "Moroz", "Neurology", "LIC-002", "0442345678")
        {
            WorkStartHour = 9,
            WorkEndHour = 18
        });
        clinic.Doctors.Add(new Doctor("Andrii", "Vlasenko", "Pediatrics", "LIC-003", "0443456789"));

        clinic.Appointments.Book(1, 1, DateTime.Now.AddDays(1).Date.AddHours(10), 30);
        clinic.Appointments.Book(2, 2, DateTime.Now.AddDays(1).Date.AddHours(11), 45);
        clinic.Appointments.Book(3, 3, DateTime.Now.AddDays(2).Date.AddHours(9), 20);

        while (true)
        {
            Console.WriteLine($"\n=== {clinic.Name.ToUpper()} ===");
            Console.WriteLine("1. Patients Menu");
            Console.WriteLine("2. Doctors Menu");
            Console.WriteLine("3. Appointments Menu");
            Console.WriteLine("4. Schedule for Date");
            Console.WriteLine("5. Generate Report");
            Console.WriteLine("0. Exit");
            Console.Write("Select an option: ");

            string? choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    RunPatientMenu(clinic);
                    break;
                case "2":
                    RunDoctorMenu(clinic);
                    break;
                case "3":
                    RunAppointmentMenu(clinic);
                    break;
                case "4":
                    Console.Write("Enter date (yyyy-MM-dd): ");
                    if (DateTime.TryParse(Console.ReadLine(), out DateTime scheduleDate))
                    {
                        clinic.DisplaySchedule(scheduleDate);
                    }
                    else
                    {
                        clinic.DisplaySchedule(DateTime.Now.AddDays(1).Date);
                    }
                    break;
                case "5":
                    clinic.GenerateReport();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    private static void RunPatientMenu(Clinic clinic)
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
                    clinic.Patients.DisplayAll();
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
                    clinic.Patients.Add(new Patient(fn, ln, DateTime.Today.AddYears(-age), blood, phone));
                    break;
                case "3":
                    Console.Write("Enter first or last name: ");
                    string q = Console.ReadLine() ?? "";
                    var found = clinic.Patients.FindByName(q);
                    if (found.Length == 0) Console.WriteLine("No one found.");
                    else foreach (var p in found) Console.WriteLine(p);
                    break;
                case "4":
                    Console.Write("Patient ID to delete: ");
                    if (int.TryParse(Console.ReadLine(), out int id))
                    {
                        if (clinic.Patients.Remove(id)) Console.WriteLine($"Patient #{id} removed.");
                        else Console.WriteLine($"Patient #{id} not found.");
                    }
                    break;
                case "5":
                    clinic.Patients.DisplayStats();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }

    private static void RunDoctorMenu(Clinic clinic)
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
                    clinic.Doctors.DisplayAll();
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
                    clinic.Doctors.Add(new Doctor(fn, ln, sp, lic, ph) { WorkStartHour = start, WorkEndHour = end });
                    break;
                case "3":
                    Console.Write("Speciality to search: ");
                    string sQuery = Console.ReadLine() ?? "";
                    var docs = clinic.Doctors.FindBySpeciality(sQuery);
                    if (docs.Length == 0) Console.WriteLine("No doctors found.");
                    else foreach (var d in docs) Console.WriteLine(d);
                    break;
                case "4":
                    Console.Write("Doctor ID to delete: ");
                    if (int.TryParse(Console.ReadLine(), out int id))
                    {
                        if (clinic.Doctors.Remove(id)) Console.WriteLine($"Doctor #{id} removed.");
                        else Console.WriteLine($"Doctor #{id} not found.");
                    }
                    break;
                case "5":
                    clinic.Doctors.DisplayStats();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }

    private static void RunAppointmentMenu(Clinic clinic)
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
                    clinic.Appointments.DisplayList(clinic.Appointments.GetUpcoming());
                    break;

                case "2":
                    Console.WriteLine("--- Available patients ---");
                    clinic.Patients.DisplayAll();
                    Console.Write("Enter patient ID: ");
                    int.TryParse(Console.ReadLine(), out int pId);

                    Console.WriteLine("--- Available doctors ---");
                    clinic.Doctors.DisplayAll();
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

                    clinic.Appointments.Book(pId, dId, dt, dur);
                    break;

                case "3":
                    Console.Write("Enter appointment ID to cancel: ");
                    int.TryParse(Console.ReadLine(), out int cancelId);
                    Console.Write("Reason: ");
                    string reason = Console.ReadLine() ?? "";
                    clinic.Appointments.Cancel(cancelId, reason);
                    break;

                case "4":
                    Console.Write("Enter appointment ID to complete: ");
                    int.TryParse(Console.ReadLine(), out int compId);
                    clinic.Appointments.Complete(compId);
                    break;

                case "5":
                    Console.Write("Enter patient ID: ");
                    int.TryParse(Console.ReadLine(), out int searchPId);
                    clinic.Appointments.DisplayList(clinic.Appointments.GetByPatient(searchPId));
                    break;

                case "6":
                    Console.Write("Enter doctor ID: ");
                    int.TryParse(Console.ReadLine(), out int searchDId);
                    clinic.Appointments.DisplayList(clinic.Appointments.GetByDoctor(searchDId));
                    break;

                case "7":
                    Console.Write("Enter date (yyyy-MM-dd): ");
                    if (DateTime.TryParse(Console.ReadLine(), out DateTime searchDate))
                    {
                        clinic.Appointments.DisplayList(clinic.Appointments.GetByDate(searchDate));
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