using System;
public static class Task3
{
    public static void Run()
    {
        string[] days = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        int[] counts = new int[7];
        int total = 0, maxIdx = 0, minIdx = 0;
        for (int i = 0; i < 7; i++)
        {
            counts[i] = int.Parse(Console.ReadLine()!);
            total += counts[i];
            if (counts[i] > counts[maxIdx]) maxIdx = i;
            if (counts[i] < counts[minIdx]) minIdx = i;
        }
        for (int i = 0; i < 7; i++)
        {
            Console.WriteLine($"{days[i],-12}: {counts[i]} patients");
        }
        Console.WriteLine($"Total:        {total}");
        Console.WriteLine($"Most:         {days[maxIdx]} ({counts[maxIdx]})");
        Console.WriteLine($"Least:        {days[minIdx]} ({counts[minIdx]})");
    }
}