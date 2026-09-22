using System;

namespace ClinicApp;

public class PatientManager
{
    private const int MaxPatients = 100;
    private Patient[] _patients = new Patient[MaxPatients];
    private int _count = 0;

    public int Count => _count;

    public void Add(Patient patient)
    {
        if (_count >= MaxPatients)
        {
            Console.WriteLine("Error: Maximum capacity of patients reached.");
            return;
        }
        _patients[_count] = patient;
        _count++;
        Console.WriteLine($"Patient [{patient.Id}] {patient.FullName} added.");
    }

    public Patient? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_patients[i].Id == id) return _patients[i];
        }
        return null;
    }

    public Patient[] FindByName(string name)
    {
        string search = name.ToLower();
        int matchCount = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_patients[i].FirstName.ToLower().Contains(search) ||
                _patients[i].LastName.ToLower().Contains(search))
            {
                matchCount++;
            }
        }

        Patient[] result = new Patient[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_patients[i].FirstName.ToLower().Contains(search) ||
                _patients[i].LastName.ToLower().Contains(search))
            {
                result[index++] = _patients[i];
            }
        }
        return result;
    }

    public bool Remove(int id)
    {
        int indexToRemove = -1;
        for (int i = 0; i < _count; i++)
        {
            if (_patients[i].Id == id)
            {
                indexToRemove = i;
                break;
            }
        }

        if (indexToRemove == -1) return false;

        for (int i = indexToRemove; i < _count - 1; i++)
        {
            _patients[i] = _patients[i + 1];
        }
        _patients[_count - 1] = null!;
        _count--;
        return true;
    }

    public void DisplayAll()
    {
        Console.WriteLine($"\n=== Patients ({_count} / {MaxPatients}) ===");
        if (_count == 0)
        {
            Console.WriteLine("No patients.");
            return;
        }
        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine(_patients[i].ToString());
        }
        Console.WriteLine(new string('-', 60));
    }

    public void DisplayStats()
    {
        Console.WriteLine("\n=== Patient Statistics ===");
        if (_count == 0)
        {
            Console.WriteLine("No patients available for statistics.");
            Console.WriteLine(new string('=', 28));
            return;
        }

        double sumAge = 0;
        int minAgeIdx = 0;
        int maxAgeIdx = 0;
        int adultCount = 0;

        for (int i = 0; i < _count; i++)
        {
            sumAge += _patients[i].Age;
            if (_patients[i].Age < _patients[minAgeIdx].Age) minAgeIdx = i;
            if (_patients[i].Age > _patients[maxAgeIdx].Age) maxAgeIdx = i;
            if (_patients[i].IsAdult) adultCount++;
        }

        double averageAge = sumAge / _count;

        Console.WriteLine($"Total:        {_count}");
        Console.WriteLine($"Average age:  {averageAge:F1} yrs");
        Console.WriteLine($"Youngest:     {_patients[minAgeIdx].FullName} ({_patients[minAgeIdx].Age} yrs)");
        Console.WriteLine($"Oldest:       {_patients[maxAgeIdx].FullName} ({_patients[maxAgeIdx].Age} yrs)");
        Console.WriteLine($"Adults:       {adultCount} of {_count}");
        Console.WriteLine(new string('=', 28));
    }
}