using System;

namespace Lab02
{
    public class Task7
    {
        public static void Run()
        {
            int n = int.Parse(Console.ReadLine()!);

            string[] names = new string[n];
            double[] bmis = new double[n];

            for (int i = 0; i < n; i++)
            {
                string[] data = Console.ReadLine()!.Split(' ');

                names[i] = data[0];
                bmis[i] = double.Parse(data[1]);
            }

            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - 1 - i; j++)
                {
                    if (bmis[j] < bmis[j + 1])
                    {
                        double tempBmi = bmis[j];
                        bmis[j] = bmis[j + 1];
                        bmis[j + 1] = tempBmi;

                        string tempName = names[j];
                        names[j] = names[j + 1];
                        names[j + 1] = tempName;
                    }
                }
            }

            Console.WriteLine("Raiting BMI");

            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"#{i + 1} {names[i]}: {bmis[i]:F2}");
            }
        }
    }
}
