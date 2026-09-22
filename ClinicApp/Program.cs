using System;

namespace ClinicApp;

class Program
{
    static void Main()
    {
        Appointment a1 = new Appointment(1, 1, new DateTime(2026, 5, 9, 10, 0, 0));
        Appointment a2 = new Appointment(2, 2, new DateTime(2026, 5, 9, 11, 0, 0), 45);
        Appointment a3 = new Appointment(3, 3, new DateTime(2026, 5, 10, 9, 0, 0), 20);

        Console.WriteLine(a1.ToString());
        Console.WriteLine(a2.ToString());
        Console.WriteLine(a3.ToString());

        Console.WriteLine("\nAfter Cancel and Complete:");

        a1.Cancel("Patient could not attend");
        a2.Complete();

        Console.WriteLine(a1.ToString());
        Console.WriteLine(a2.ToString());
        Console.WriteLine(a3.ToString());
    }
}