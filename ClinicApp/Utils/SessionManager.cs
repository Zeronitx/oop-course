using System;
using System.Globalization;
using System.IO;
using System.Text;
using ClinicApp.Enums;
using ClinicApp.Models;

namespace ClinicApp.Utils;

public class SessionManager
{
    private readonly string _sessionPath;

    public SessionManager(string sessionPath = "session.dat")
    {
        _sessionPath = sessionPath;
    }

    public bool Exists()
    {
        return File.Exists(_sessionPath);
    }

    public void Save(Clinic clinic)
    {
        Patient[] patients = clinic.Patients.GetAll();
        using StreamWriter writer = new StreamWriter(_sessionPath, false, Encoding.UTF8);

        writer.WriteLine("[PATIENTS]");
        for (int i = 0; i < patients.Length; i++)
        {
            Patient p = patients[i];
            writer.WriteLine($"{p.FirstName},{p.LastName},{p.DateOfBirth:dd.MM.yyyy},{p.BloodType},{p.Phone}");
        }
        writer.WriteLine("[END]");
    }

    public int Load(Clinic clinic)
    {
        if (!Exists()) return 0;

        string[] lines = File.ReadAllLines(_sessionPath, Encoding.UTF8);
        string currentSection = "";
        int loadedCount = 0;

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrWhiteSpace(line)) continue;

            if (line.StartsWith("["))
            {
                currentSection = line;
                if (currentSection == "[END]") break;
                continue;
            }

            if (currentSection == "[PATIENTS]")
            {
                try
                {
                    string[] parts = line.Split(',');
                    if (parts.Length != 5) continue;

                    string firstName = parts[0].Trim();
                    string lastName = parts[1].Trim();
                    DateTime dateOfBirth = DateTime.ParseExact(parts[2].Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    BloodType bloodType = (BloodType)Enum.Parse(typeof(BloodType), parts[3].Trim());
                    string phone = parts[4].Trim();

                    if (PatientExists(clinic, firstName, lastName, dateOfBirth)) continue;

                    Patient patient = new Patient(firstName, lastName, dateOfBirth, bloodType, phone);
                    clinic.Patients.Add(patient);
                    loadedCount++;
                }
                catch
                {
                }
            }
        }

        return loadedCount;
    }

    private bool PatientExists(Clinic clinic, string firstName, string lastName, DateTime dateOfBirth)
    {
        Patient[] existing = clinic.Patients.GetAll();
        for (int i = 0; i < existing.Length; i++)
        {
            Patient p = existing[i];
            if (p.FirstName == firstName && p.LastName == lastName && p.DateOfBirth.Date == dateOfBirth.Date)
            {
                return true;
            }
        }
        return false;
    }
}