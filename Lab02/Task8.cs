using System;
public static class Task8
{
    public static void Run()
    {
        int d = int.Parse(Console.ReadLine()!);
        int w = int.Parse(Console.ReadLine()!);
        int[,,] patients = new int[d, w, 2];
        int[] deptTotals = new int[d];
        for (int i = 0; i < d; i++)
        {
            for (int j = 0; j < w; j++)
            {
                for (int k = 0; k < 2; k++) patients[i, j, k] = int.Parse(Console.ReadLine()!);
            }
        }
        int maxTotal = 0, maxDept = 0;
        for (int i = 0; i < d; i++)
        {
            Console.WriteLine($"Department {i + 1}:");
            for (int j = 0; j < w; j++)
            {
                int morning = patients[i, j, 0];
                int evening = patients[i, j, 1];
                int weekTotal = morning + evening;
                deptTotals[i] += weekTotal;
                Console.WriteLine($"  Week {j + 1}: morning {morning}, evening {evening} -> total {weekTotal}");
            }
            Console.WriteLine($"  Total: {deptTotals[i]} patients");
            if (deptTotals[i] > maxTotal)
            {
                maxTotal = deptTotals[i];
                maxDept = i;
            }
        }
        Console.WriteLine($"Most loaded: Department {maxDept + 1} ({maxTotal} patients)");
    }
}