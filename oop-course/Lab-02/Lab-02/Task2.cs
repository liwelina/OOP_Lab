using System;

namespace Lab02
{
    public class Task2
    {
        public static void Run()
        {
            int n = int.Parse(Console.ReadLine()!);

            int[] queue = new int[n];

            for (int i = 0; i < n; i++)
            {
                queue[i] = int.Parse(Console.ReadLine()!);
            }

            string before = string.Join(" ", queue);

            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - 1 - i; j++)
                {
                    if (queue[j] > queue[j + 1])
                    {
                        int temp = queue[j];
                        queue[j] = queue[j + 1];
                        queue[j + 1] = temp;
                    }
                }
            }

            string after = string.Join(" ", queue);

            Console.WriteLine($"Queue before: {before}");
            Console.WriteLine($"Queue after: {after}");
            Console.WriteLine($"Cheapest: {queue[0]} uah");
            Console.WriteLine($"Most expensive: {queue[n - 1]} uah");
        }
    }
}