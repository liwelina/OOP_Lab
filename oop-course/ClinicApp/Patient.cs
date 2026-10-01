namespace ClinicApp;

public class Patient
{
    private static int _nextId = 1;

    public int Id { get; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public BloodType BloodType { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }

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
        Id = _nextId++;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dob;
        BloodType = bloodType;
        Phone = phone;
        Email = email;
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

