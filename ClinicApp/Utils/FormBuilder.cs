using System;
using System.Collections.Generic;
using System.Reflection;
using ClinicApp.Attributes;

namespace ClinicApp.Utils;

public static class FormBuilder
{
    public static T Build<T>() where T : new()
    {
        T obj = new T();
        PropertyInfo[] properties = typeof(T).GetProperties();

        for (int i = 0; i < properties.Length; i++)
        {
            PropertyInfo prop = properties[i];
            if (prop.GetSetMethod() == null) continue;

            List<string> hints = new List<string>();
            if (prop.GetCustomAttribute<RequiredAttribute>() != null)
            {
                hints.Add("обов'язкове");
            }

            MaxLengthAttribute? maxLength = prop.GetCustomAttribute<MaxLengthAttribute>();
            if (maxLength != null)
            {
                hints.Add($"макс. {maxLength.Length}");
            }

            MinValueAttribute? minValue = prop.GetCustomAttribute<MinValueAttribute>();
            if (minValue != null)
            {
                hints.Add($"мін. {minValue.Min}");
            }

            string hintText = hints.Count > 0 ? $" ({string.Join(", ", hints)})" : "";

            while (true)
            {
                Console.Write($"  {prop.Name}{hintText}: ");
                string input = Console.ReadLine() ?? "";

                try
                {
                    object converted = Convert.ChangeType(input, prop.PropertyType);
                    prop.SetValue(obj, converted);
                    break;
                }
                catch
                {
                    Console.WriteLine("  Некоректне значення, спробуйте ще раз.");
                }
            }
        }

        return obj;
    }
}