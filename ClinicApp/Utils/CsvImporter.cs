using System;
using System.Globalization;
using System.IO;
using System.Text;
using ClinicApp.Enums;
using ClinicApp.Models;

namespace ClinicApp.Utils;

public class CsvImporter
{
    public static ImportResult ImportPatients(Clinic clinic, string filePath)
    {
        ImportResult result = new ImportResult();

        if (!File.Exists(filePath))
        {
            result.AddError(0, $"Файл не знайдено: {filePath}");
            return result;
        }

        string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];
            if (string.IsNullOrWhiteSpace(line)) continue;

            int lineNumber = i + 1;
            try
            {
                string[] parts = line.Split(',');
                if (parts.Length != 5)
                {
                    result.AddError(lineNumber, $"Рядок має {parts.Length} полів замість 5.");
                    continue;
                }

                string firstName = parts[0].Trim();
                string lastName = parts[1].Trim();
                DateTime birthDate = DateTime.ParseExact(parts[2].Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture);
                BloodType bloodType = (BloodType)Enum.Parse(typeof(BloodType), parts[3].Trim());
                string phone = parts[4].Trim();

                Patient patient = new Patient(firstName, lastName, birthDate, bloodType, phone);
                clinic.Patients.Add(patient);
                result.AddSuccess();
            }
            catch (Exception ex)
            {
                result.AddError(lineNumber, ex.Message);
            }
        }

        return result;
    }
}