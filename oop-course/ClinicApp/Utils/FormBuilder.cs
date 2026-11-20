using System.Reflection;
using ClinicApp.Attributes;

namespace ClinicApp.Utils
{
    public static class FormBuilder
    {
        public static T Build<T>() where T : new()
        {
            T instance = new T();
            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (PropertyInfo prop in properties)
            {
                if (prop.GetSetMethod() == null)
                {
                    continue;
                }

                string hint = BuildHint(prop);

                while (true)
                {
                    if (hint.Length > 0)
                    {
                        Console.Write($"  {prop.Name} ({hint}): ");
                    }
                    else
                    {
                        Console.Write($"  {prop.Name}: ");
                    }

                    string? input = Console.ReadLine();

                    try
                    {
                        object? converted = Convert.ChangeType(input ?? "", prop.PropertyType);
                        prop.SetValue(instance, converted);
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("  Некоректне значення, спробуйте ще раз.");
                    }
                }
            }

            return instance;
        }

        private static string BuildHint(PropertyInfo prop)
        {
            List<string> parts = new List<string>();

            RequiredAttribute? requiredAttr = prop.GetCustomAttribute<RequiredAttribute>();
            if (requiredAttr != null)
            {
                parts.Add("обов'язкове");
            }

            MaxLengthAttribute? maxLengthAttr = prop.GetCustomAttribute<MaxLengthAttribute>();
            if (maxLengthAttr != null)
            {
                parts.Add($"макс. {maxLengthAttr.Length}");
            }

            MinValueAttribute? minValueAttr = prop.GetCustomAttribute<MinValueAttribute>();
            if (minValueAttr != null)
            {
                parts.Add($"мін. {minValueAttr.Min}");
            }

            string result = "";
            for (int i = 0; i < parts.Count; i++)
            {
                if (i > 0)
                {
                    result += ", ";
                }
                result += parts[i];
            }
            return result;
        }
    }
}