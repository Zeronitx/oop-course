using System;
using System.Reflection;
using ClinicApp.Attributes;

namespace ClinicApp.Utils;

public static class ModelValidator
{
    public static ValidationResult Validate(object obj)
    {
        ValidationResult result = new ValidationResult();
        PropertyInfo[] properties = obj.GetType().GetProperties();

        for (int i = 0; i < properties.Length; i++)
        {
            PropertyInfo prop = properties[i];
            object? value = prop.GetValue(obj);

            RequiredAttribute? required = prop.GetCustomAttribute<RequiredAttribute>();
            if (required != null)
            {
                if (value == null || (value is string s && string.IsNullOrWhiteSpace(s)))
                {
                    result.AddError($"{prop.Name}: {required.ErrorMessage}");
                }
            }

            MaxLengthAttribute? maxLength = prop.GetCustomAttribute<MaxLengthAttribute>();
            if (maxLength != null && value is string str && str.Length > maxLength.Length)
            {
                result.AddError($"{prop.Name}: {maxLength.ErrorMessage}");
            }

            MinValueAttribute? minValue = prop.GetCustomAttribute<MinValueAttribute>();
            if (minValue != null && value != null)
            {
                double num = Convert.ToDouble(value);
                if (num < minValue.Min)
                {
                    result.AddError($"{prop.Name}: {minValue.ErrorMessage}");
                }
            }
        }

        return result;
    }

    public static void PrintInfo(Type type)
    {
        Console.WriteLine(type.Name);
        PropertyInfo[] properties = type.GetProperties();

        for (int i = 0; i < properties.Length; i++)
        {
            PropertyInfo prop = properties[i];
            string attrs = "";

            foreach (Attribute attr in prop.GetCustomAttributes())
            {
                string name = attr.GetType().Name;
                if (name.EndsWith("Attribute"))
                {
                    name = name.Substring(0, name.Length - "Attribute".Length);
                }
                attrs += $" [{name}]";
            }

            Console.WriteLine($"  {prop.Name,-14}{attrs}");
        }
    }
}