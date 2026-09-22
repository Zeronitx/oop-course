using System;

namespace ClinicApp;

class Program
{
    static void Main()
    {
        Doctor d1 = new Doctor("Oleh", "Sydorenko", "Cardiology", "LIC-001", "0441234567");
        d1.WorkEndHour = 16;

        Doctor d2 = new Doctor("Natalia", "Moroz", "Neurology", "LIC-002", "0442345678");
        d2.WorkStartHour = 9;
        d2.WorkEndHour = 18;

        Doctor d3 = new Doctor("Andriy", "Vlasenko", "Pediatrics");

        Console.WriteLine(d1.ToString());
        Console.WriteLine(d2.ToString());
        Console.WriteLine(d3.ToString());
    }
}