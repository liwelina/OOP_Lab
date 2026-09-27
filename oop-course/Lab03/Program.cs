using System.Text;

namespace Lab03;

internal class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        PatientManager manager = new PatientManager();

        manager.Add(new Patient("Ivan", "Petrenko", DateTime.Today.AddYears(-41), "A+", "0501234567"));
        manager.Add(new Patient("Olena", "Koval", DateTime.Today.AddYears(-33), "B-", "0672345678"));
        manager.Add(new Patient("Maksym", "Boyko", DateTime.Today.AddYears(-16), "O+", "0933456789"));
        manager.Add(new Patient("Maria", "Tkach"));

        RunPatientMenu(manager);
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
            Console.WriteLine("0. Exit");
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
                        Console.WriteLine("No one found");
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
}