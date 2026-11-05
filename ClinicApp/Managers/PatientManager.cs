using System;
using System.Collections.Generic;
using ClinicApp.Models;
using ClinicApp.Enums;

namespace ClinicApp.Managers;

public class PatientManager
{
    private List<Patient> _patients = new List<Patient>();

    public int Count => _patients.Count;

    public void Add(Patient patient)
    {
        _patients.Add(patient);
        Console.WriteLine($"Patient [{patient.Id}] {patient.FullName} added.");
    }

    public void Remove(int id)
    {
        for (int i = 0; i < _patients.Count; i++)
        {
            if (_patients[i].Id == id)
            {
                _patients.RemoveAt(i);
                Console.WriteLine($"Patient [{id}] removed.");
                return;
            }
        }
        Console.WriteLine("Error: Patient not found.");
    }

    public Patient? FindById(int id)
    {
        for (int i = 0; i < _patients.Count; i++)
        {
            if (_patients[i].Id == id) return _patients[i];
        }
        return null;
    }

    public Patient[] FindByName(string name)
    {
        List<Patient> result = new List<Patient>();
        for (int i = 0; i < _patients.Count; i++)
        {
            if (_patients[i].FirstName.Contains(name, StringComparison.OrdinalIgnoreCase) ||
                _patients[i].LastName.Contains(name, StringComparison.OrdinalIgnoreCase))
            {
                result.Add(_patients[i]);
            }
        }
        return result.ToArray();
    }

    public Patient[] FindByBloodType(BloodType bloodType)
    {
        List<Patient> result = new List<Patient>();
        for (int i = 0; i < _patients.Count; i++)
        {
            if (_patients[i].BloodType == bloodType)
            {
                result.Add(_patients[i]);
            }
        }
        return result.ToArray();
    }

    public Patient[] GetAll()
    {
        return _patients.ToArray();
    }

    public void DisplayAll()
    {
        Console.WriteLine($"\n=== All Patients ({_patients.Count}) ===");
        if (_patients.Count == 0)
        {
            Console.WriteLine("No patients found.");
            return;
        }

        for (int i = 0; i < _patients.Count; i++)
        {
            Console.WriteLine(_patients[i].ToString());
        }
        Console.WriteLine(new string('-', 60));
    }

    public void DisplayStats()
    {
        Console.WriteLine("\n=== Patient Statistics ===");
        Console.WriteLine($"Total Patients: {_patients.Count}");
        if (_patients.Count == 0) return;

        int totalAge = 0;
        int adults = 0;
        for (int i = 0; i < _patients.Count; i++)
        {
            totalAge += _patients[i].Age;
            if (_patients[i].IsAdult) adults++;
        }

        Console.WriteLine($"Average Age: {(double)totalAge / _patients.Count:F1}");
        Console.WriteLine($"Adults: {adults} | Children: {_patients.Count - adults}");
    }
}