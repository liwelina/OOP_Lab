using System.Text;

namespace ClinicApp;

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

        string patientName = clinic.Patients.FindById(99)?.FullName ?? "не знайдено";
        Console.WriteLine($"Patient with ID 99: {patientName}");

        bool running = true;

        while (running)
        {
            Console.WriteLine();
            Console.WriteLine("=== CLINIC MENU ===");
            Console.WriteLine("1. Show patients");
            Console.WriteLine("2. Show doctors");
            Console.WriteLine("3. Show appointments");
            Console.WriteLine("4. Search doctor by speciality");
            Console.WriteLine("5. Add patient");
            Console.WriteLine("6. Add doctor");
            Console.WriteLine("7. Show statistics");
            Console.WriteLine("0. Exit");
            Console.Write("Choose: ");

            int choice = int.Parse(Console.ReadLine()!);

            Console.WriteLine();

            switch (choice)
            {
                case 1:
                    clinic.Patients.DisplayAll();
                    break;

                case 2:
                    clinic.Doctors.DisplayAll();
                    break;

                case 3:
                    clinic.Appointments.DisplayList(
                        clinic.Appointments.GetUpcoming());
                    break;

                case 4:
                    Console.WriteLine("=== Specialities ===");

                    int specialityNumber = 0;

                    foreach (Speciality speciality in Enum.GetValues<Speciality>())
                    {
                        Console.WriteLine($"{specialityNumber}. {speciality}");
                        specialityNumber++;
                    }

                    Console.Write("Choose speciality number: ");
                    int selectedSpeciality = int.Parse(Console.ReadLine()!);

                    Speciality specialityValue = (Speciality)selectedSpeciality;

                    Doctor[] doctors = clinic.Doctors.FindBySpeciality(
                        specialityValue);

                    if (doctors.Length == 0)
                    {
                        Console.WriteLine("No doctors found.");
                    }
                    else
                    {
                        foreach (Doctor doctor in doctors)
                        {
                            Console.WriteLine(doctor);
                        }
                    }

                    break;

                case 5:
                    Console.Write("First name: ");
                    string firstName = Console.ReadLine()!;

                    Console.Write("Last name: ");
                    string lastName = Console.ReadLine()!;

                    Console.Write("Phone: ");
                    string phone = Console.ReadLine()!;

                    Console.WriteLine("=== Blood types ===");

                    int bloodTypeNumber = 0;

                    foreach (BloodType bloodType in Enum.GetValues<BloodType>())
                    {
                        Console.WriteLine($"{bloodTypeNumber}. {bloodType}");
                        bloodTypeNumber++;
                    }

                    Console.Write("Choose blood type number: ");
                    int selectedBloodType = int.Parse(Console.ReadLine()!);

                    BloodType bloodTypeValue = (BloodType)selectedBloodType;

                    Patient newPatient = new Patient(
                        firstName,
                        lastName,
                        DateTime.Today.AddYears(-25),
                        bloodTypeValue,
                        phone);

                    clinic.Patients.Add(newPatient);
                    break;

                case 6:
                    Console.Write("First name: ");
                    string doctorFirstName = Console.ReadLine()!;

                    Console.Write("Last name: ");
                    string doctorLastName = Console.ReadLine()!;

                    Console.Write("License number: ");
                    string licenseNumber = Console.ReadLine()!;

                    Console.Write("Phone: ");
                    string doctorPhone = Console.ReadLine()!;

                    Console.WriteLine("=== Specialities ===");

                    int doctorSpecialityNumber = 0;

                    foreach (Speciality speciality in Enum.GetValues<Speciality>())
                    {
                        Console.WriteLine($"{doctorSpecialityNumber}. {speciality}");
                        doctorSpecialityNumber++;
                    }

                    Console.Write("Choose speciality number: ");
                    int doctorSelectedSpeciality = int.Parse(Console.ReadLine()!);

                    Speciality doctorSpecialityValue =
                        (Speciality)doctorSelectedSpeciality;

                    Doctor newDoctor = new Doctor(
                        doctorFirstName,
                        doctorLastName,
                        doctorSpecialityValue,
                        licenseNumber,
                        doctorPhone);

                    clinic.Doctors.Add(newDoctor);
                    break;

                case 7:
                    clinic.Patients.DisplayStats();
                    Console.WriteLine();
                    clinic.Doctors.DisplayStats();
                    break;

                case 0:
                    running = false;
                    break;

                default:
                    Console.WriteLine("Unknown menu option.");
                    break;
            }
        }

        Console.WriteLine("Program finished.");
    }
}
