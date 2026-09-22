using System;

namespace ClinicApp;

class Program
{
    static void Main()
    {
        Clinic clinic = new Clinic("Medical Clinic");

        // Додаємо пацієнтів через клініку
        clinic.Patients.Add(new Patient("Ivan", "Petrenko", new DateTime(1985, 5, 10), "A+", "0501234567"));
        clinic.Patients.Add(new Patient("Olena", "Koval", new DateTime(1993, 8, 22), "B-", "0672345678"));
        clinic.Patients.Add(new Patient("Maksym", "Boiko", new DateTime(2010, 1, 15), "O+", "0933456789"));
        clinic.Patients.Add(new Patient("Maria", "Tkach"));

        // Додаємо лікарів через клініку
        clinic.Doctors.Add(new Doctor("Oleh", "Sydorenko", "Cardiology"));
        clinic.Doctors.Add(new Doctor("Natalia", "Moroz", "Neurology"));
        clinic.Doctors.Add(new Doctor("Andriy", "Vlasenko", "Pediatrics"));

        // Робимо записи
        clinic.Appointments.Book(1, 1, new DateTime(2026, 5, 9, 10, 0, 0));
        clinic.Appointments.Book(2, 2, new DateTime(2026, 5, 9, 11, 0, 0), 45);
        clinic.Appointments.Book(3, 3, new DateTime(2026, 5, 10, 9, 0, 0), 20);

        // Виводимо розклад та звіт
        clinic.DisplaySchedule(new DateTime(2026, 5, 9));
        clinic.GenerateReport();
    }
}