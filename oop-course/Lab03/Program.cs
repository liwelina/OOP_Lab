using System.Text;

namespace Lab03;

internal class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        Appointment app1 = new Appointment(1, 1, new DateTime(2026, 5, 9, 10, 0, 0), 30);
        Appointment app2 = new Appointment(2, 2, new DateTime(2026, 5, 9, 11, 0, 0), 45);
        Appointment app3 = new Appointment(3, 3, new DateTime(2026, 5, 10, 9, 0, 0), 20);

        Console.WriteLine(app1);
        Console.WriteLine(app2);
        Console.WriteLine(app3);

        app1.Cancel("The patient was unable to come");
        app2.Complete();

        Console.WriteLine("\n// After Cancel та Complete:");
        Console.WriteLine(app1);
        Console.WriteLine(app2);
    }
}