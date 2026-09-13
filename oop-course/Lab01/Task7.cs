using System;

namespace Lab01
{
    public class Task7
    {
        public static void Run()
        {
            Console.Write("Enter number of appointments: ");
            int n = Convert.ToInt32(Console.ReadLine());

            double sum = 0;

            for (int i = 1; i <= n; i++)
            {
                Console.Write("Enter cost of appointment " + i + ": ");
                double price = Convert.ToDouble(Console.ReadLine());

                sum = sum + price;
            }

            double average = sum / n;

            Console.WriteLine("Number of appointments: " + n);
            Console.WriteLine("Total cost: " + sum);
            Console.WriteLine("Average cost: " + average);
        }
    }
}


