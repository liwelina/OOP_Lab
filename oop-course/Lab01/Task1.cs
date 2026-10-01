using System;

namespace Lab01
{
    public class Task1
    {
        public static void Run()
        {
            Console.Write("Enter weight: ");
            double weight = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter height: ");
            double height = Convert.ToDouble(Console.ReadLine());
            height = height / 100;

           double bmi = weight / (height * height);
            Console.WriteLine("Body Mass Index: " + bmi);
        }
    }
}