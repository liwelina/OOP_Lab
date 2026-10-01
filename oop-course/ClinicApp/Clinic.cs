namespace ClinicApp;

public class Clinic
{
    public string Name { get; set; }
    public PatientManager Patients { get; }
    public DoctorManager Doctors { get; }
    public AppointmentManager Appointments { get; }

    public Clinic(string name)
    {
        Name = name;
        Patients = new PatientManager();
        Doctors = new DoctorManager();
        Appointments = new AppointmentManager(Patients, Doctors);
    }

    public void DisplaySchedule(DateTime date)
    {
        Console.WriteLine($"=== Schedule for {date:dd.MM.yyyy} ===");
        Appointment[] dailyApps = Appointments.GetByDate(date);
        Appointments.DisplayList(dailyApps);
    }

    public void GenerateReport()
    {
        Appointment[] upcomingApps = Appointments.GetUpcoming();
        Doctor[] allDoctors = Doctors.GetAll();

        Console.WriteLine("ã==============================================¬");
        Console.WriteLine($"¦  Report — {Name,-34} ¦");
        Console.WriteLine("¦==============================================¦");
        Console.WriteLine($"¦  Patients:              {Patients.Count,-20} ¦");
        Console.WriteLine($"¦  Doctors:               {Doctors.Count,-20} ¦");
        Console.WriteLine($"¦  Upcoming appointments: {upcomingApps.Length,-20} ¦");
        Console.WriteLine("¦==============================================¦");
        Console.WriteLine("¦  Doctors load (upcoming appointments):       ¦");

        for (int i = 0; i < allDoctors.Length; i++)
        {
            int doctorLoad = 0;
            for (int j = 0; j < upcomingApps.Length; j++)
            {
                if (upcomingApps[j].DoctorId == allDoctors[i].Id)
                {
                    doctorLoad++;
                }
            }

            string doctorInfo = $"{allDoctors[i].FullName} ({allDoctors[i].Speciality}): {doctorLoad} apps";
            Console.WriteLine($"¦    {doctorInfo,-41} ¦");
        }

        Console.WriteLine("L==============================================-");
    }
}

