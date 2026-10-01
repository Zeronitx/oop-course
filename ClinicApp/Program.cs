using System;

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

        clinic.Doctors.Add(new Doctor("Oleh", "Sydorenko", Speciality.Cardiology));
        clinic.Doctors.Add(new Doctor("Natalia", "Moroz", Speciality.Neurology));
        clinic.Doctors.Add(new Doctor("Andriy", "Vlasenko", Speciality.Pediatrics));

        clinic.Appointments.Book(1, 1, new DateTime(2026, 5, 9, 10, 0, 0));
        clinic.Appointments.Book(2, 2, new DateTime(2026, 5, 9, 11, 0, 0), 45);
        clinic.Appointments.Book(3, 3, new DateTime(2026, 5, 10, 9, 0, 0), 20);

        clinic.DisplaySchedule(new DateTime(2026, 5, 9));
        clinic.GenerateReport();
    }
}