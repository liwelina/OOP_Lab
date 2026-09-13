using System;

namespace Lab01
{
    public class Task8
    {
        public static double CalculateBMI(double weight, double height)
        {
            height = height / 100;
            return weight / (height * height);
        }

        public static string GetBMICategory(double bmi)
        {
            if (bmi < 18.5)
            {
                return "underweight";
            }
            else if (bmi < 25)
            {
                return "normal weight";
            }
            else if (bmi < 30)
            {
                return "overweight";
            }
            else
            {
                return "obesity";
            }
        }

        public static double CalculateCost(double price, int quantity, int discount)
        {
            return price * quantity * (1 - discount / 100.0);
        }

        public static string GetAgeCategory(int age)
        {
            if (age <= 17)
            {
                return "child";
            }
            else if (age <= 59)
            {
                return "adult";
            }
            else
            {
                return "pensioner";
            }
        }

        public static string GetPressureStatus(int systolic, int diastolic)
        {
            if (systolic < 120 && diastolic < 80)
            {
                return "normal";
            }
            else if (systolic < 130 && diastolic < 80)
            {
                return "elevated";
            }
            else if (systolic < 140 || diastolic < 90)
            {
                return "hypertension stage 1";
            }
            else
            {
                return "hypertension stage 2";
            }
        }

        public static void Run()
        {
            Console.Write("Enter weight: ");
            double weight = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter height: ");
            double height = Convert.ToDouble(Console.ReadLine());

            double bmi = CalculateBMI(weight, height);

            Console.WriteLine("Body Mass Index: " + bmi);
            Console.WriteLine("BMI category: " + GetBMICategory(bmi));


            Console.Write("Enter price: ");
            double price = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter quantity: ");
            int quantity = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter discount: ");
            int discount = Convert.ToInt32(Console.ReadLine());

            double sum = CalculateCost(price, quantity, discount);

            Console.WriteLine("Total cost: " + sum);


            Console.Write("Enter year of birth: ");
            int birthYear = Convert.ToInt32(Console.ReadLine());

            int age = 2026 - birthYear;

            Console.WriteLine("Age: " + age);
            Console.WriteLine("Category: " + GetAgeCategory(age));


            Console.Write("Enter systolic pressure: ");
            int systolic = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter diastolic pressure: ");
            int diastolic = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Category: " + GetPressureStatus(systolic, diastolic));
        }
    }
}


