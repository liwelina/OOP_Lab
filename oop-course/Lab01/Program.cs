using System;

namespace Lab01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Write("Введіть номер завдання (1-8): ");
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1": Task1.Run(); break;
                case "2": Task2.Run(); break;
                case "3": Task3.Run(); break;
                case "4": Task4.Run(); break;
                case "5": Task5.Run(); break;
                case "6": Task6.Run(); break;
                case "7": Task7.Run(); break;
                case "8":
                   
                    break;
                default:
                    Console.WriteLine("Завдання не знайдено.");
                    break;
            }
        }
    }
}