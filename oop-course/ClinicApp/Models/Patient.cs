using ClinicApp.Utils;
using ClinicApp.Enums;

namespace ClinicApp.Models;

public class Patient
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

    private DateTime _dateOfBirth;
    public DateTime DateOfBirth
    {
        get => _dateOfBirth;
        set
        {
            ClinicValidator.ValidateDate(value, nameof(DateOfBirth));
            _dateOfBirth = value;
        }
    }

    public BloodType BloodType { get; set; }

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

    private string _email = "";
    public string Email
    {
        get => _email;
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                ClinicValidator.ValidateEmail(value);
            }
            _email = value;
        }
    }

    public string FullName => $"{FirstName} {LastName}";

    public int Age
    {
        get
        {
            var today = DateTime.Today;
            int age = today.Year - DateOfBirth.Year;

            if (DateOfBirth.Date > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }
    }

    public bool IsAdult => Age >= 18;

    public Patient() : this("Unknown", "Patient")
    {
    }

    public Patient(string firstName, string lastName)
        : this(firstName, lastName, DateTime.Today.AddYears(-26), BloodType.Unknown, "0000000000", "")
    {
    }

    public Patient(string firstName, string lastName, DateTime dob, BloodType bloodType, string phone, string email = "")
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dob;
        BloodType = bloodType;
        Phone = phone;
        Email = email;
        Id = _nextId++;
    }

    public string GetAgeCategory()
    {
        if (Age < 18)
        {
            return "Child";
        }
        else if (Age < 60)
        {
            return "Adult";
        }
        else
        {
            return "Pensioner";
        }
    }

    public override string ToString()
    {
        return $"[{Id}] {FullName} | Age: {ClinicFormatter.FormatAge(Age)} ({GetAgeCategory()}) | Blood: {ClinicFormatter.FormatBloodType(BloodType)} | Phone: {ClinicFormatter.FormatPhone(Phone)}";
    }
}
