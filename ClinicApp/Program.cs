using System;
using ClinicApp;
using ClinicApp.Models;
using ClinicApp.Managers;
using ClinicApp.Enums;

namespace ClinicApp;

class Program
{
    static void Main()
    {
        Clinic clinic = new Clinic("Medical Clinic");

        clinic.Patients.Add(new Patient("Ivan", "Petrenko", new DateTime(1985, 5, 10), BloodType.APositive, "0501234567"));
        clinic.Patients.Add(new Patient("Olena", "Koval", new DateTime(1993, 8, 22), BloodType.BNegative, "0672345678"));
        clinic.Patients.Add(new Patient("Maksym", "Boiko", new DateTime(2010, 1, 15), BloodType.OPositive, "0933456789"));
        clinic.Patients.Add(new Patient("Maria", "Tkach"));

        Doctor d1 = new Doctor("Oleh", "Sydorenko", Speciality.Cardiology);
        d1.Schedule = new WorkSchedule(8, 16);

        Doctor d2 = new Doctor("Natalia", "Moroz", Speciality.Neurology);
        d2.Schedule = new WorkSchedule(9, 18);

        Doctor d3 = new Doctor("Andriy", "Vlasenko", Speciality.Pediatrics);

        clinic.Doctors.Add(d1);
        clinic.Doctors.Add(d2);
        clinic.Doctors.Add(d3);

        clinic.Appointments.Book(1, 1, new DateTime(2026, 5, 9, 10, 0, 0));
        clinic.Appointments.Book(2, 2, new DateTime(2026, 5, 9, 11, 0, 0), 45);
        clinic.Appointments.Book(3, 3, new DateTime(2026, 5, 10, 9, 0, 0), 20);

        Console.WriteLine("\n=== Testing Task 4 ===");

        Doctor[] cardiologists = clinic.Doctors.FindBySpeciality(Speciality.Cardiology);
        Console.WriteLine($"Found {cardiologists.Length} cardiologists using enum.");

        Appointment[] todayApps = clinic.Appointments.GetByDate(2026, 5, 9);
        Console.WriteLine($"Found {todayApps.Length} appointments for 2026-05-09 using overloaded GetByDate.");

        if (clinic.Patients.TryFindById(3, out Patient p))
        {
            Console.WriteLine($"TryFindById success: {p.FullName}");
        }

        if (!clinic.Doctors.TryFindById(99, out Doctor d))
        {
            Console.WriteLine("TryFindById properly handled non-existent doctor.");
        }

        string missingName = clinic.Patients.FindById(99)?.FullName ?? "Patient not found";
        Console.WriteLine($"Testing ?. and ??: {missingName}");
    }
}