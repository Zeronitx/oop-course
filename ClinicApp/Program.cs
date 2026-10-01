using System;

namespace ClinicApp;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Testing struct WorkSchedule ===");
        WorkSchedule morning = new WorkSchedule(8, 16);
        WorkSchedule copy = morning;

        Console.WriteLine($"Morning: {morning}");
        Console.WriteLine($"Copy:    {copy}");
        Console.WriteLine($"Is it working time now for morning shift? {morning.IsNow}\n");

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

        clinic.DisplaySchedule(new DateTime(2026, 5, 9));
        clinic.GenerateReport();
    }
}