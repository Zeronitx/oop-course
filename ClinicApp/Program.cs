using System;

namespace ClinicApp;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== GrowablePatientManager Test ===");
        Console.WriteLine("Adding patients one by one...");

        GrowablePatientManager manager = new GrowablePatientManager();

        for (int i = 1; i <= 20; i++)
        {
            Patient p = new Patient("Test", $"Patient{i}");
            manager.Add(p);
        }

        Console.WriteLine("\nSearch test:");
        Patient? found = manager.FindById(10);
        Console.WriteLine($"  FindById(10) -> {(found != null ? found.FullName : "not found")}");

        Patient? notFound = manager.FindById(99);
        Console.WriteLine($"  FindById(99) -> {(notFound != null ? notFound.FullName : "not found")}");

        Console.WriteLine("\nComparison:");
        Console.WriteLine("  PatientManager:         100 places (fixed)");
        Console.WriteLine($"  GrowablePatientManager:  {manager.Capacity} places (grows when needed)");
    }
}