using System;

namespace Lab01
{
    public class Task4
    {
        public static void Run()
        {
            Console.Write("Enter systolic pressure: ");
            int systolic = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter diastolic pressure: ");
            int diastolic = Convert.ToInt32(Console.ReadLine());

            if (systolic < 120 && diastolic < 80)
            {
                Console.WriteLine("Category: normal");
            }
            else if (systolic < 130 && diastolic < 80)
            {
                Console.WriteLine("Category: elevated");
            }
            else if (systolic < 140 || diastolic < 90)
            {
                Console.WriteLine("Category: hypertension stage 1");
            }
            else
            {
                Console.WriteLine("Category: hypertension stage 2");
            }
        }
    }
}
