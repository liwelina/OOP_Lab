using System.Text;

namespace Lab03;

internal class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        Patient patient1 = new Patient("Іванка", "Шевченко", DateTime.Today.AddYears(-41), "A+", "0501234567");
        Patient patient2 = new Patient("Максим", "Оленіч", DateTime.Today.AddYears(-33), "B-", "0672345678");
        Patient patient3 = new Patient("Марина", "Сеник", DateTime.Today.AddYears(-16), "O+", "0933456789");

        Patient patient4 = new Patient();

        Patient patient5 = new Patient("Карина", "Деркач");

        Patient[] patients = { patient1, patient2, patient3, patient4, patient5 };

        foreach (var patient in patients)
        {
            Console.WriteLine(patient);
        }
    }
}