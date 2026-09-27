using System.Text;

namespace Lab03;

internal class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        Console.WriteLine("=== Test GrowablePatientManager ===");
        Console.WriteLine("We are adding patients one by one...");

        GrowablePatientManager growableManager = new GrowablePatientManager();

        for (int i = 1; i <= 20; i++)
        {
            Patient p = new Patient($"Test", $"Patient{i}");
            growableManager.Add(p);
        }

        Console.WriteLine("Search test:");
        Patient? found10 = growableManager.FindById(10);
        Console.WriteLine($"  FindById(10) → {(found10 != null ? found10.FullName : "not found")}");

        Patient? found99 = growableManager.FindById(99);
        Console.WriteLine($"  FindById(99) → {(found99 != null ? found99.FullName : "not found")}");

        Console.WriteLine("\nComparison:");
        Console.WriteLine($"  PatientManager:         100 seats (fixed))");
        Console.WriteLine($"  GrowablePatientManager:  {growableManager.Capacity} spaces (will increase if necessary)");
    }
}