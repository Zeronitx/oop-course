using System;
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

        Doctor d1 = new Doctor("Oleh", "Sydorenko", Speciality.Cardiology);
        clinic.Doctors.Add(d1);

        Console.WriteLine("\n=== Testing Try/Catch (Lab05 Task04) ===");

        try
        {
            Console.WriteLine("Спроба додати пацієнта з пустим ім'ям...");
            Patient badPatient = new Patient("", "Test", DateTime.Today, BloodType.Unknown, "0000000000");
            clinic.Patients.Add(badPatient);
        }
        catch (ArgumentOutOfRangeException e)
        {
            Console.WriteLine($"Помилка даних (діапазон): {e.Message}");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Помилка даних (формат): {e.Message}");
        }

        try
        {
            Console.WriteLine("\nСпроба встановити лікареві графік з 20:00 до 06:00...");
            Doctor badDoctor = new Doctor("Test", "Test", Speciality.Emergency);
            badDoctor.Schedule = new WorkSchedule(20, 6);
            clinic.Doctors.Add(badDoctor);
        }
        catch (ArgumentOutOfRangeException e)
        {
            Console.WriteLine($"Помилка графіку (діапазон): {e.Message}");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Помилка графіку (формат): {e.Message}");
        }

        try
        {
            Console.WriteLine("\nСпроба записати на прийом із тривалістю -15 хвилин...");
            clinic.Appointments.Book(1, 1, new DateTime(2026, 5, 9, 10, 0, 0), -15);
        }
        catch (ArgumentOutOfRangeException e)
        {
            Console.WriteLine($"Помилка запису (діапазон): {e.Message}");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Помилка запису (формат): {e.Message}");
        }

        Console.WriteLine("\nПрограма продовжує роботу штатно, падінь не було!");
    }
}