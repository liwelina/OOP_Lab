using System;

namespace Lab01
{
    public class Task6
    {
        public static void Run()
        {
            Console.Write("Enter medical card number: ");
            int number = Convert.ToInt32(Console.ReadLine());

            int lastDigit = number % 10;

            if (lastDigit == 0 || lastDigit == 1)
            {
                Console.WriteLine("Department: general therapy");
            }
            else if (lastDigit == 2 || lastDigit == 3)
            {
                Console.WriteLine("Department: surgery");
            }
            else if (lastDigit == 4 || lastDigit == 5)
            {
                Console.WriteLine("Department: cardiology");
            }
            else if (lastDigit == 6 || lastDigit == 7)
            {
                Console.WriteLine("Department: neurology");
            }
            else
            {
                Console.WriteLine("Department: ophthalmology");
            }

            if (number % 2 == 0)
            {
                Console.WriteLine("Preferential card: yes");
            }

            if (number % 3 == 0)
            {
                Console.WriteLine("Scheduled examination: yes");
            }
        }
    }
}