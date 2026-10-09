using System;
using ClinicApp.Enums;
using ClinicApp.Utils;

namespace ClinicApp.Models;

public class Patient
{
    private static int _nextId = 1;

    private string _firstName = "";
    private string _lastName = "";
    private DateTime _dateOfBirth;
    private string _phone = "";

    public int Id { get; }

    public string FirstName
    {
        get => _firstName;
        set
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length > 50)
                throw new ArgumentException("Invalid First Name.", nameof(FirstName));
            _firstName = value;
        }
    }

    public string LastName
    {
        get => _lastName;
        set
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length > 50)
                throw new ArgumentException("Invalid Last Name.", nameof(LastName));
            _lastName = value;
        }
    }

    public DateTime DateOfBirth
    {
        get => _dateOfBirth;
        set
        {
            if (value.Date > DateTime.Today || value.Year < 1900)
                throw new ArgumentOutOfRangeException(nameof(DateOfBirth), "Date of Birth must be between 1900 and today.");
            _dateOfBirth = value;
        }
    }

    public BloodType BloodType { get; set; }

    public string Phone
    {
        get => _phone;
        set
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length != 10)
                throw new ArgumentException("Phone must be exactly 10 characters.", nameof(Phone));
            foreach (char c in value)
            {
                if (!char.IsDigit(c))
                    throw new ArgumentException("Phone must contain only digits.", nameof(Phone));
            }
            _phone = value;
        }
    }

    public string Email { get; set; } = string.Empty;

    public string FullName => $"{FirstName} {LastName}";

    public int Age
    {
        get
        {
            int age = DateTime.Today.Year - DateOfBirth.Year;
            if (DateOfBirth.Date > DateTime.Today.AddYears(-age))
            {
                age--;
            }
            return age;
        }
    }

    public bool IsAdult => Age >= 18;

    public Patient(string firstName, string lastName, DateTime dob, BloodType bloodType, string phone)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dob;
        BloodType = bloodType;
        Phone = phone;

        Id = _nextId++;
    }

    public Patient(string firstName, string lastName)
        : this(firstName, lastName, DateTime.Today, BloodType.Unknown, "0000000000")
    {
    }

    public Patient()
        : this("Unknown", "Patient")
    {
    }

    public string GetAgeCategory()
    {
        if (Age < 18) return "child";
        if (Age < 60) return "adult";
        return "senior";
    }

    public override string ToString()
    {
        return $"[{Id}] {FullName} | Age: {ClinicFormatter.FormatAge(Age)} ({GetAgeCategory()}) | Blood: {ClinicFormatter.FormatBloodType(BloodType)} | Tel: {ClinicFormatter.FormatPhone(Phone)}";
    }
}