using System;

namespace Lab01
{
    public class Task3
    {
        public static void Run()
        {
            Console.Write("Enter year of birth: ");
            int birthYear = Convert.ToInt32(Console.ReadLine());

            int age = 2026 - birthYear;

            if (age <= 17)
            {
                Console.WriteLine("Age: " + age);
                Console.WriteLine("Category: child");
            }
            else if (age <= 59)
            {
                Console.WriteLine("Age: " + age);
                Console.WriteLine("Category: adult");
            }
            else
            {
                Console.WriteLine("Age: " + age);
                Console.WriteLine("Category: pensioner");
            }
        }
    }
}