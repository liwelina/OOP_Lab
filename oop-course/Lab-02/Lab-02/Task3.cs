using System;

namespace Lab02
{
    public class Task3
    {
        public static void Run()
        {
            string[] days =
            {
                "Monday",
                "Tuesday",
                "Wednesday",
                "Thursday",
                "Friday",
                "Saturday",
                "Sunday"
            };

            int[] counts = new int[7];

            for (int i = 0; i < 7; i++)
            {
                counts[i] = int.Parse(Console.ReadLine()!);
            }

            int total = 0;
            int maxIdx = 0;
            int minIdx = 0;

            for (int i = 0; i < 7; i++)
            {
                total += counts[i];

                if (counts[i] > counts[maxIdx])
                {
                    maxIdx = i;
                }

                if (counts[i] < counts[minIdx])
                {
                    minIdx = i;
                }
            }

            for (int i = 0; i < 7; i++)
            {
                Console.WriteLine($"{days[i],-12}: {counts[i]} patients");
            }

            Console.WriteLine($"Total:        {total}");
            Console.WriteLine($"Max:    {days[maxIdx]} ({counts[maxIdx]})");
            Console.WriteLine($"Min:     {days[minIdx]} ({counts[minIdx]})");
        }
    }
}