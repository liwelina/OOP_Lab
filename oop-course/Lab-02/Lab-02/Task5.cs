using System;

namespace Lab02
{
    public class Task5
    {
        public static void Run()
        {
            int n = int.Parse(Console.ReadLine()!);

            int[,] matrix = new int[n, n];

            for (int i = 0; i < n; i++)
            {
                string[] values = Console.ReadLine()!.Split(' ');

                for (int j = 0; j < n; j++)
                {
                    matrix[i, j] = int.Parse(values[j]);
                }
            }

            int[] mainDiagonal = new int[n];
            int[] secondaryDiagonal = new int[n];

            int mainSum = 0;
            int secondarySum = 0;

            for (int i = 0; i < n; i++)
            {
                mainDiagonal[i] = matrix[i, i];
                secondaryDiagonal[i] = matrix[i, n - 1 - i];

                mainSum += mainDiagonal[i];
                secondarySum += secondaryDiagonal[i];
            }

            Console.WriteLine(
                $"main diagonal: {string.Join(", ", mainDiagonal)} (sum = {mainSum})"
            );

            Console.WriteLine(
                $"secondary diagonal: {string.Join(", ", secondaryDiagonal)} (sum = {secondarySum})"
            );
        }
    }
}