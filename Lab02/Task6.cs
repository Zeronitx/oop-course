using System;
public static class Task6
{
    public static void Run()
    {
        int n = int.Parse(Console.ReadLine()!);
        int[][] costs = new int[n][];
        int maxIncome = 0, bestDoctorIdx = 0;
        for (int i = 0; i < n; i++)
        {
            int k = int.Parse(Console.ReadLine()!);
            costs[i] = new int[k];
            int sum = 0;
            for (int j = 0; j < k; j++)
            {
                costs[i][j] = int.Parse(Console.ReadLine()!);
                sum += costs[i][j];
            }
            double avg = (double)sum / k;
            Console.WriteLine($"Doctor {i + 1}: {k} appointments, sum={sum} UAH, average={avg:F2} UAH");
            if (sum > maxIncome)
            {
                maxIncome = sum;
                bestDoctorIdx = i;
            }
        }
        Console.WriteLine($"Highest income: Doctor {bestDoctorIdx + 1} ({maxIncome} UAH)");
    }
}