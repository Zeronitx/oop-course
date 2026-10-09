using System;
using System.Text.RegularExpressions;

namespace ClinicApp.Utils;

public static class ClinicValidator
{
    private static readonly Regex PhoneRegex = new Regex(@"^(?:\+38)?[0-9]{10}\z");
    private static readonly Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+\z");

    public static void ValidateName(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 50)
            throw new ArgumentException("Name cannot be empty or longer than 50 characters.", fieldName);
    }

    public static void ValidatePhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone) || !PhoneRegex.IsMatch(phone))
            throw new ArgumentException("Phone must be exactly 10 digits (or start with +38).", nameof(phone));
    }

    public static void ValidateEmail(string email)
    {
        if (!string.IsNullOrEmpty(email) && !EmailRegex.IsMatch(email))
            throw new ArgumentException("Invalid email format.", nameof(email));
    }

    public static void ValidateDate(DateTime value, string fieldName)
    {
        if (value.Date > DateTime.Today || value.Year < 1900)
            throw new ArgumentOutOfRangeException(fieldName, "Date must be between 1900 and today.");
    }

    public static void ValidatePositive(int value, string fieldName)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(fieldName, "Value must be greater than 0.");
    }
}