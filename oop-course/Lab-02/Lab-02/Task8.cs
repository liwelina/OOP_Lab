using System;

namespace Lab02
{
    public class Task8
    {
        public static void Run()
        {
            int departments = int.Parse(Console.ReadLine()!);
            int weeks = int.Parse(Console.ReadLine()!);

            int[,,] patients = new int[departments, weeks, 2];
            int[] totals = new int[departments];

            for (int i = 0; i < departments; i++)
            {
                for (int j = 0; j < weeks; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        patients[i, j, k] = int.Parse(Console.ReadLine()!);
                        totals[i] += patients[i, j, k];
                    }
                }
            }

            int maxTotal = totals[0];
            int maxDepartment = 0;

            for (int i = 0; i < departments; i++)
            {
                Console.WriteLine($"Department {i + 1}:");

                for (int j = 0; j < weeks; j++)
                {
                    int morning = patients[i, j, 0];
                    int evening = patients[i, j, 1];
                    int weekTotal = morning + evening;

                    Console.WriteLine(
                        $"  Week {j + 1}: morning {morning}, evening {evening} -> total {weekTotal}"
                    );
                }

                Console.WriteLine($"  Total: {totals[i]} patients");

                if (totals[i] > maxTotal)
                {
                    maxTotal = totals[i];
                    maxDepartment = i;
                }
            }

            Console.WriteLine(
                $"Busiest department: Department {maxDepartment + 1} ({maxTotal} patients)"
            );
        }
    }
}
