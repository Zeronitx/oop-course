using System;

namespace ClinicApp;

class Program
{
    static void Main()
    {
        Patient p1 = new Patient("Ivan", "Petrenko", new DateTime(1985, 5, 10), "A+", "0501234567");
        Patient p2 = new Patient("Olena", "Koval", new DateTime(1993, 8, 22), "B-", "0672345678");
        Patient p3 = new Patient("Maksym", "Boiko", new DateTime(2010, 1, 15), "O+", "0933456789");
        Patient p4 = new Patient("Unknown", "Patient", new DateTime(2000, 1, 1), "Unknown", "0000000000");
        Patient p5 = new Patient("Maria", "Tkach");

        Console.WriteLine(p1.ToString());
        Console.WriteLine(p2.ToString());
        Console.WriteLine(p3.ToString());
        Console.WriteLine(p4.ToString());
        Console.WriteLine(p5.ToString());
    }
}