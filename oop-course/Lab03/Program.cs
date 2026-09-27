namespace Lab03;

internal class Program
{
    static void Main(string[] args)
    {
        Doctor doc1 = new Doctor("Oleh", "Sydorenko", "Cardiology", "LIC-001", "0441234567")
        {
            WorkStartHour = 8,
            WorkEndHour = 16
        };

        Doctor doc2 = new Doctor("Nataliia", "Moroz", "Neurology", "LIC-002", "0442345678")
        {
            WorkStartHour = 9,
            WorkEndHour = 18
        };

        Doctor doc3 = new Doctor("Andrii", "Vlasenko", "Pediatrics", "LIC-003", "0443456789");

        Doctor doc4 = new Doctor("Svitlana", "Melnyk", "Therapy");

        Doctor[] doctors = { doc1, doc2, doc3, doc4 };

        foreach (var doctor in doctors)
        {
            Console.WriteLine(doctor);
        }
    }
}