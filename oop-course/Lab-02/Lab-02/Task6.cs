using System;

namespace Lab02
{
    public class Task6
    {
        public static void Run()
        {
            int n = int.Parse(Console.ReadLine()!);

            int[][] costs = new int[n][];

            for (int i = 0; i < n; i++)
            {
                int k = int.Parse(Console.ReadLine()!);

                costs[i] = new int[k];

                for (int j = 0; j < k; j++)
                {
                    costs[i][j] = int.Parse(Console.ReadLine()!);
                }
            }

            int maxIncome = 0;
            int maxDoctor = 0;

            for (int i = 0; i < n; i++)
            {
                int sum = 0;

                for (int j = 0; j < costs[i].Length; j++)
                {
                    sum += costs[i][j];
                }

                double average = (double)sum / costs[i].Length;

                Console.WriteLine(
                    $"Doctor {i + 1}: {costs[i].Length} appointment, sum={sum} uah, average={average:F2} uah"
                );

                if (sum > maxIncome)
                {
                    maxIncome = sum;
                    maxDoctor = i;
                }
            }

            Console.WriteLine(
                $"Highest income: Doctor {maxDoctor + 1} ({maxIncome} uah)"
            );
        }
    }
}
