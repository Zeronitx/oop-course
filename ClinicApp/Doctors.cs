using System;

namespace ClinicApp;

public class Doctor
{
    private static int _nextId = 1;

    public int Id { get; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Speciality Speciality { get; set; }
    public string LicenseNumber { get; set; }
    public string Phone { get; set; }

    public WorkSchedule Schedule { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    public bool IsAvailableNow => Schedule.IsNow;

    public Doctor(string firstName, string lastName, Speciality speciality, string licenseNumber, string phone)
    {
        Id = _nextId++;
        FirstName = firstName;
        LastName = lastName;
        Speciality = speciality;
        LicenseNumber = licenseNumber;
        Phone = phone;
        Schedule = new WorkSchedule(8, 17);
    }

    public Doctor(string firstName, string lastName, Speciality speciality)
        : this(firstName, lastName, speciality, "Unknown", "0000000000")
    {
    }

    public Doctor()
        : this("Unknown", "Doctor", Speciality.General)
    {
    }

    public bool CanAcceptAt(int hour)
    {
        return Schedule.Contains(hour);
    }

    public override string ToString()
    {
        string status = IsAvailableNow ? "available now" : "not in working hours";
        return $"[{Id}] {FullName} | {Speciality} | {LicenseNumber} | Phone: {Phone} | {Schedule} | {status}";
    }
}