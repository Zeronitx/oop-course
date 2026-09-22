using System;

namespace ClinicApp;

class Program
{
    static void Main()
    {
        DoctorManager manager = new DoctorManager();

        Doctor d1 = new Doctor("Oleh", "Sydorenko", "Cardiology", "LIC-001", "0441234567");
        d1.WorkEndHour = 16;
        Doctor d2 = new Doctor("Natalia", "Moroz", "Neurology", "LIC-002", "0442345678");
        d2.WorkStartHour = 9;
        d2.WorkEndHour = 18;
        Doctor d3 = new Doctor("Andriy", "Vlasenko", "Pediatrics");
        Doctor d4 = new Doctor("Anna", "Koval", "Cardiology", "LIC-004", "0445556677");

        manager.Add(d1);
        manager.Add(d2);
        manager.Add(d3);
        manager.Add(d4);

        DoctorsMenu(manager);
    }

    static void DoctorsMenu(DoctorManager manager)
    {
        manager.DisplayAll();
        manager.DisplayStats();

        Console.WriteLine("\nSearching for 'Cardiology':");
        Doctor[] found = manager.FindBySpeciality("Cardiology");
        foreach (var d in found)
        {
            Console.WriteLine(d.ToString());
        }

        Console.WriteLine("\nRemoving doctor with ID 4...");
        manager.Remove(4);
        manager.DisplayStats();
    }
}