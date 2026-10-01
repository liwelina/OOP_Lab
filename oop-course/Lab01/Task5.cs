using System;

namespace Lab01
{
    public class Task5
    {
        public static void Run()
        {
            Console.Write("Enter day number (1-7): ");
            int day = Convert.ToInt32(Console.ReadLine());

            switch (day)
            {
                case 1:
                    Console.WriteLine("Monday: 08:00-18:00");
                    break;
                case 2:
                    Console.WriteLine("Tuesday: 08:00-18:00");
                    break;
                case 3:
                    Console.WriteLine("Wednesday: 09:00-17:00");
                    break;
                case 4:
                    Console.WriteLine("Thursday: 08:00-18:00");
                    break;
                case 5:
                    Console.WriteLine("Friday: 08:00-16:00");
                    break;
                case 6:
                    Console.WriteLine("Saturday: 09:00-14:00");
                    break;
                case 7:
                    Console.WriteLine("Sunday: day off");
                    break;
                default:
                    Console.WriteLine("Invalid day number");
                    break;
            }
        }
    }
}