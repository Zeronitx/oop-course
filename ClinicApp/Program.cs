using System;

namespace ClinicApp;

class Program
{
    static void Main()
    {
        // 1. Створюємо менеджери та додаємо початкові дані
        PatientManager patients = new PatientManager();
        patients.Add(new Patient("Ivan", "Petrenko", new DateTime(1985, 5, 10), "A+", "0501234567"));
        patients.Add(new Patient("Olena", "Koval", new DateTime(1993, 8, 22), "B-", "0672345678"));
        patients.Add(new Patient("Maksym", "Boiko", new DateTime(2010, 1, 15), "O+", "0933456789"));

        DoctorManager doctors = new DoctorManager();
        doctors.Add(new Doctor("Oleh", "Sydorenko", "Cardiology"));
        doctors.Add(new Doctor("Natalia", "Moroz", "Neurology"));
        doctors.Add(new Doctor("Andriy", "Vlasenko", "Pediatrics"));

        // 2. Створюємо AppointmentManager, передаючи йому існуючі менеджери
        AppointmentManager appointments = new AppointmentManager(patients, doctors);

        Console.WriteLine("\n--- Booking Appointments ---");
        appointments.Book(1, 1, new DateTime(2026, 5, 9, 10, 0, 0));
        appointments.Book(2, 2, new DateTime(2026, 5, 9, 11, 0, 0), 45);
        appointments.Book(3, 3, new DateTime(2026, 5, 10, 9, 0, 0), 20);

        // Тест на неіснуючого пацієнта (повинна бути помилка)
        appointments.Book(99, 1, new DateTime(2026, 5, 10, 10, 0, 0));

        Console.WriteLine("\n--- Upcoming Appointments ---");
        appointments.DisplayList(appointments.GetUpcoming());

        Console.WriteLine("\n--- Cancelling Appointment [1] ---");
        appointments.Cancel(1, "Patient changed mind");

        Console.WriteLine("\n--- Patient #2 Appointments ---");
        appointments.DisplayList(appointments.GetByPatient(2));
    }
}