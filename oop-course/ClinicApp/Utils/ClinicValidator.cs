using System;
using System.Text.RegularExpressions;

namespace ClinicApp.Utils;

public static class ClinicValidator
{
    private static readonly Regex PhoneRegex = new Regex(@"^[0-9]{10}\z", RegexOptions.Compiled);
    private static readonly Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+\z", RegexOptions.Compiled);

    public static void ValidateName(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 50)
        {
            throw new ArgumentException($"{fieldName} cannot be empty or exceed 50 characters.", fieldName);
        }
    }

    public static void ValidatePhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone) || !PhoneRegex.IsMatch(phone))
        {
            throw new ArgumentException("Phone number must contain exactly 10 ASCII digits.", nameof(phone));
        }
    }

    public static void ValidateEmail(string email)
    {
        if (!EmailRegex.IsMatch(email))
        {
            throw new ArgumentException("Invalid email format.", nameof(email));
        }
    }

    public static void ValidateDate(DateTime value, string fieldName)
    {
        if (value > DateTime.Today || value.Year < 1900)
        {
            throw new ArgumentOutOfRangeException(fieldName, "Date of birth cannot be in the future or earlier than year 1900.");
        }
    }

    public static void ValidatePositive(int value, string fieldName)
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(fieldName, $"{fieldName} must be greater than zero.");
        }
    }
}