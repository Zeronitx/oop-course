using System;

namespace ClinicApp.Utils;

public static class ClinicValidator
{
    public static void ValidateName(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 50)
            throw new ArgumentException("Name cannot be empty or longer than 50 characters.", fieldName);
    }

    public static void ValidatePhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone) || phone.Length != 10)
            throw new ArgumentException("Phone must be exactly 10 characters.", nameof(phone));

        foreach (char c in phone)
        {
            if (!char.IsDigit(c))
                throw new ArgumentException("Phone must contain only digits.", nameof(phone));
        }
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