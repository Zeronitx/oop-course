using System;
public static class Task4
{
    public static void Run()
    {
        int n = int.Parse(Console.ReadLine()!);
        int m = int.Parse(Console.ReadLine()!);
        int[,] matrix = new int[n, m];
        for (int i = 0; i < n; i++)
        {
            string[] parts = Console.ReadLine()!.Split(' ');
            for (int j = 0; j < m; j++) matrix[i, j] = int.Parse(parts[j]);
        }
        int maxVal = matrix[0, 0], maxRow = 0, maxCol = 0;
        for (int i = 0; i < n; i++)
        {
            int rowSum = 0;
            for (int j = 0; j < m; j++)
            {
                rowSum += matrix[i, j];
                if (matrix[i, j] > maxVal)
                {
                    maxVal = matrix[i, j];
                    maxRow = i;
                    maxCol = j;
                }
            }
            Console.WriteLine($"Doctor {i + 1}: {rowSum} appointments");
        }
        int[] colSums = new int[m];
        for (int j = 0; j < m; j++)
        {
            for (int i = 0; i < n; i++) colSums[j] += matrix[i, j];
        }
        Console.WriteLine($"By days: {string.Join(", ", colSums)}");
        Console.WriteLine($"Maximum: {maxVal} (Doctor {maxRow + 1}, Day {maxCol + 1})");
    }
}