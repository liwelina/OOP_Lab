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

        patientManager.Add(new Patient("Ivan", "Petrenko", DateTime.Today.AddYears(-41), "A+", "0501234567"));
        patientManager.Add(new Patient("Olena", "Koval", DateTime.Today.AddYears(-33), "B-", "0672345678"));
        patientManager.Add(new Patient("Maksym", "Boyko", DateTime.Today.AddYears(-16), "O+", "0933456789"));
        patientManager.Add(new Patient("Maria", "Tkach"));
        patientManager.Add(new Patient("Vasyl", "Shevchenko", DateTime.Today.AddYears(-52), "AB+", "0681122334"));
        patientManager.Add(new Patient("Anna", "Melnyk", DateTime.Today.AddYears(-24), "A-", "0955566778"));
        patientManager.Add(new Patient("Dmytro", "Bondar", DateTime.Today.AddYears(-70), "O-", "0639988776"));

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
        doctorManager.Add(new Doctor("Iryna", "Kravets", "Cardiology", "LIC-004", "0444567890"));

        while (true)
        {
            Console.WriteLine("\n=== CLINIC SYSTEM ===");
            Console.WriteLine("1. Patients Menu");
            Console.WriteLine("2. Doctors Menu");
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
                    Console.Write("Name: ");
                    string firstName = Console.ReadLine() ?? "";
                    Console.Write("Surname: ");
                    string lastName = Console.ReadLine() ?? "";
                    Console.Write("Age: ");
                    int.TryParse(Console.ReadLine(), out int age);
                    Console.Write("Blood type: ");
                    string blood = Console.ReadLine() ?? "Unknown";
                    Console.Write("Phone: ");
                    string phone = Console.ReadLine() ?? "0000000000";

                    manager.Add(new Patient(firstName, lastName, DateTime.Today.AddYears(-age), blood, phone));
                    break;

                case "3":
                    Console.Write("Enter a first name or surname to search: ");
                    string query = Console.ReadLine() ?? "";
                    var found = manager.FindByName(query);

                    if (found.Length == 0)
                    {
                        Console.WriteLine("No one found.");
                    }
                    else
                    {
                        Console.WriteLine($"Found ({found.Length}):");
                        foreach (var p in found)
                        {
                            Console.WriteLine(p);
                        }
                    }
                    break;

                case "4":
                    Console.Write("Enter the ID to delete: ");
                    if (int.TryParse(Console.ReadLine(), out int id))
                    {
                        if (manager.Remove(id))
                        {
                            Console.WriteLine($"Patient with ID {id} successfully removed.");
                        }
                        else
                        {
                            Console.WriteLine($"Patient with ID {id} not found.");
                        }
                    }
                    break;

                case "5":
                    manager.DisplayStats();
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Wrong choice.");
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
                    Console.Write("Name: ");
                    string firstName = Console.ReadLine() ?? "";
                    Console.Write("Surname: ");
                    string lastName = Console.ReadLine() ?? "";
                    Console.Write("Speciality: ");
                    string speciality = Console.ReadLine() ?? "";
                    Console.Write("License: ");
                    string license = Console.ReadLine() ?? "";
                    Console.Write("Phone: ");
                    string phone = Console.ReadLine() ?? "";
                    Console.Write("Work start hour (0-23): ");
                    int.TryParse(Console.ReadLine(), out int startHour);
                    Console.Write("Work end hour (0-23): ");
                    int.TryParse(Console.ReadLine(), out int endHour);

                    Doctor doc = new Doctor(firstName, lastName, speciality, license, phone)
                    {
                        WorkStartHour = startHour,
                        WorkEndHour = endHour
                    };
                    manager.Add(doc);
                    break;

                case "3":
                    Console.Write("Enter speciality to search: ");
                    string specQuery = Console.ReadLine() ?? "";
                    var foundDoctors = manager.FindBySpeciality(specQuery);

                    if (foundDoctors.Length == 0)
                    {
                        Console.WriteLine("No doctors found with this speciality.");
                    }
                    else
                    {
                        Console.WriteLine($"Found ({foundDoctors.Length}):");
                        foreach (var d in foundDoctors)
                        {
                            Console.WriteLine(d);
                        }
                    }
                    break;

                case "4":
                    Console.Write("Enter doctor ID to delete: ");
                    if (int.TryParse(Console.ReadLine(), out int id))
                    {
                        if (manager.Remove(id))
                        {
                            Console.WriteLine($"Doctor with ID {id} successfully removed.");
                        }
                        else
                        {
                            Console.WriteLine($"Doctor with ID {id} not found.");
                        }
                    }
                    break;

                case "5":
                    manager.DisplayStats();
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Wrong choice.");
                    break;
            }
        }
    }
}