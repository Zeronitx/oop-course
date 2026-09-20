using System;
public static class Task5
{
    public static void Run()
    {
        int n = int.Parse(Console.ReadLine()!);
        int[,] matrix = new int[n, n];
        for (int i = 0; i < n; i++)
        {
            string[] parts = Console.ReadLine()!.Split(' ');
            for (int j = 0; j < n; j++) matrix[i, j] = int.Parse(parts[j]);
        }
        int[] mainDiag = new int[n], secDiag = new int[n];
        int mainSum = 0, secSum = 0;
        for (int i = 0; i < n; i++)
        {
            mainDiag[i] = matrix[i, i];
            mainSum += matrix[i, i];
            secDiag[i] = matrix[i, n - 1 - i];
            secSum += matrix[i, n - 1 - i];
        }
        Console.WriteLine($"Main diagonal: {string.Join(", ", mainDiag)} (sum = {mainSum})");
        Console.WriteLine($"Secondary diagonal: {string.Join(", ", secDiag)} (sum = {secSum})");
    }
}