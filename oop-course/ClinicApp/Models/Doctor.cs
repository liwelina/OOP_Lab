using ClinicApp.Utils;
using ClinicApp.Enums;
using ClinicApp.Interfaces;

namespace ClinicApp.Models;

public class Doctor : ISchedulable
{
    private static int _nextId = 1;

    public int Id { get; private set; }

    private string _firstName = "";
    public string FirstName
    {
        get => _firstName;
        set
        {
            ClinicValidator.ValidateName(value, nameof(FirstName));
            _firstName = value;
        }
    }

    private string _lastName = "";
    public string LastName
    {
        get => _lastName;
        set
        {
            ClinicValidator.ValidateName(value, nameof(LastName));
            _lastName = value;
        }
    }

    public Speciality Speciality { get; set; }

    private string _licenseNumber = "";
    public string LicenseNumber
    {
        get => _licenseNumber;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("License number cannot be empty or whitespace.", nameof(LicenseNumber));
            }
            _licenseNumber = value;
        }
    }

    private string _phone = "";
    public string Phone
    {
        get => _phone;
        set
        {
            ClinicValidator.ValidatePhone(value);
            _phone = value;
        }
    }

    public WorkSchedule Schedule { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    public int WorkingHoursPerDay => Schedule.HoursPerDay;

    public string WorkSchedule => Schedule.Display;

    public bool IsAvailableNow => Schedule.IsNow;

    public Doctor() : this("Unknown", "Doctor", Speciality.General)
    {
    }

    public Doctor(string firstName, string lastName, Speciality speciality)
        : this(firstName, lastName, speciality, "LIC-000", "0000000000")
    {
    }

    public Doctor(string firstName, string lastName, Speciality speciality, string licenseNumber, string phone)
    {
        FirstName = firstName;
        LastName = lastName;
        Speciality = speciality;
        LicenseNumber = licenseNumber;
        Phone = phone;
        Schedule = new WorkSchedule(8, 17);
        Id = _nextId++;
    }

    public bool CanAcceptAt(int hour)
    {
        return Schedule.Contains(hour);
    }

    // Реалізація інтерфейсу ISchedulable
    public bool CanSchedule(DateTime at)
    {
        return Schedule.Contains(at.Hour);
    }

    public DateTime[] GetAvailableSlots(DateTime date, int slotCount)
    {
        ClinicValidator.ValidatePositive(slotCount, nameof(slotCount));

        int hoursCount = Schedule.EndHour - Schedule.StartHour;
        int count = Math.Min(slotCount, hoursCount);

        DateTime[] slots = new DateTime[count];
        DateTime baseDate = date.Date;

        for (int i = 0; i < count; i++)
        {
            slots[i] = baseDate.AddHours(Schedule.StartHour + i);
        }

        return slots;
    }

    public override string ToString()
    {
        string status = IsAvailableNow ? "Available now" : "Outside working hours";

        return $"[{Id}] {FullName} | {ClinicFormatter.FormatSpeciality(Speciality)} | {LicenseNumber} | Phone: {ClinicFormatter.FormatPhone(Phone)} | {Schedule} | {status}";
    }
}