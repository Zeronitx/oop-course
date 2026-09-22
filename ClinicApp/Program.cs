using System;

namespace ClinicApp;

class Program
{
    static void Main()
    {
        PatientManager manager = new PatientManager();

        manager.Add(new Patient("Ivan", "Petrenko", new DateTime(1985, 5, 10), "A+", "0501234567"));
        manager.Add(new Patient("Olena", "Koval", new DateTime(1993, 8, 22), "B-", "0672345678"));
        manager.Add(new Patient("Maksym", "Boiko", new DateTime(2010, 1, 15), "O+", "0933456789"));
        manager.Add(new Patient("Maria", "Tkach", new DateTime(2000, 1, 1), "Unknown", "0000000000"));

        PatientsMenu(manager);
    }

    static void PatientsMenu(PatientManager manager)
    {
        manager.DisplayAll();
        manager.DisplayStats();

        Console.WriteLine("\nSearching for 'Koval':");
        Patient[] found = manager.FindByName("Koval");
        foreach (var p in found)
        {
            Console.WriteLine(p.ToString());
        }

        Console.WriteLine("\nRemoving patient with ID 4...");
        manager.Remove(4);
        manager.DisplayAll();
    }
}