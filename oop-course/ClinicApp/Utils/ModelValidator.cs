using System.Reflection;
using ClinicApp.Attributes;

namespace ClinicApp.Utils
{
    public static class ModelValidator
    {
        public static ValidationResult Validate(object obj)
        {
            ValidationResult result = new ValidationResult();

            if (obj == null)
            {
                result.AddError("Object is null.");
                return result;
            }

            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo prop in properties)
            {
                object? value = prop.GetValue(obj);

                RequiredAttribute? requiredAttr = prop.GetCustomAttribute<RequiredAttribute>();
                if (requiredAttr != null)
                {
                    bool isMissing = value == null || (value is string str && string.IsNullOrWhiteSpace(str));
                    if (isMissing)
                    {
                        result.AddError($"{prop.Name}: {requiredAttr.ErrorMessage}");
                    }
                }

                MaxLengthAttribute? maxLengthAttr = prop.GetCustomAttribute<MaxLengthAttribute>();
                if (maxLengthAttr != null && value is string text)
                {
                    if (text.Length > maxLengthAttr.Length)
                    {
                        result.AddError($"{prop.Name}: {maxLengthAttr.ErrorMessage}");
                    }
                }

                MinValueAttribute? minValueAttr = prop.GetCustomAttribute<MinValueAttribute>();
                if (minValueAttr != null && value != null)
                {
                    double numericValue = Convert.ToDouble(value);
                    if (numericValue < minValueAttr.Min)
                    {
                        result.AddError($"{prop.Name}: {minValueAttr.ErrorMessage}");
                    }
                }
            }

            return result;
        }

        public static void PrintInfo(Type type)
        {
            Console.WriteLine($"=== Type: {type.Name} ===");
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo prop in properties)
            {
                Console.WriteLine($"  Property: {prop.PropertyType.Name} {prop.Name}");

                Attribute[] attrs = prop.GetCustomAttributes().ToArray();
                foreach (Attribute attr in attrs)
                {
                    Console.WriteLine($"    - [{attr.GetType().Name}]");
                }
            }
        }
    }
}