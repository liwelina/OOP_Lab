using System;

namespace Lab01
{
    public class Task2
    {
        public static void Run()
        {
            Console.Write("Enter price: ");
            double price = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter quantity: ");
            int quantity = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter discount: ");
            double discount = Convert.ToDouble(Console.ReadLine());

            double sum = price * quantity * (1 - discount / 100);

            Console.WriteLine("Total cost: " + sum);
        }
    }
}
