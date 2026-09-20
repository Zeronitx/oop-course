using System;
public static class Task1
{
    public static void Run()
    {
        int n = int.Parse(Console.ReadLine()!);
        double[] weights = new double[n];
        for (int i = 0; i < n; i++) weights[i] = double.Parse(Console.ReadLine()!);
        double sum = 0, min = weights[0], max = weights[0];
        foreach (double weight in weights)
        {
            sum += weight;
            if (weight < min) min = weight;
            if (weight > max) max = weight;
        }
        double average = sum / n;
        int aboveAverage = 0;
        foreach (double weight in weights)
        {
            if (weight > average) aboveAverage++;
        }
        Console.WriteLine($"Ammount: {n} / Mid Weight: {average:F1} kg / Min / Max: {min:F1} / {max:F1} kg / Above average: {aboveAverage} з {n}");
    }
}