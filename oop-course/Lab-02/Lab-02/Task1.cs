using System;

namespace Lab02
{
    public class Task1
    {
        public static void Run()
        {
            int n = int.Parse(Console.ReadLine()!);

            double[] weights = new double[n];

            for (int i = 0; i < n; i++)
            {
                weights[i] = double.Parse(Console.ReadLine()!);
            }

            double sum = 0;
            double min = weights[0];
            double max = weights[0];

            foreach (double weight in weights)
            {
                sum += weight;

                if (weight < min)
                {
                    min = weight;
                }

                if (weight > max)
                {
                    max = weight;
                }
            }

            double average = sum / n;

            int aboveAverage = 0;

            foreach (double weight in weights)
            {
                if (weight > average)
                {
                    aboveAverage++;
                }
            }

            Console.WriteLine($"Quantity: {n}");
            Console.WriteLine($"Average weight: {average:F1} kg");
            Console.WriteLine($"Min / Max: {min:F1} / {max:F1} kg");
            Console.WriteLine($"Above average: {aboveAverage} з {n}");
        }
    }
}