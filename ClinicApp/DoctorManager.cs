using System;

namespace ClinicApp;

public class DoctorManager
{
    private const int MaxDoctors = 50;
    private Doctor[] _doctors = new Doctor[MaxDoctors];
    private int _count = 0;

    public int Count => _count;

    public void Add(Doctor doctor)
    {
        if (_count >= MaxDoctors)
        {
            Console.WriteLine("Error: Maximum capacity of doctors reached.");
            return;
        }
        _doctors[_count] = doctor;
        _count++;
        Console.WriteLine($"Doctor [{doctor.Id}] {doctor.FullName} added.");
    }

    public Doctor? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Id == id) return _doctors[i];
        }
        return null;
    }

    public Doctor[] FindBySpeciality(string speciality)
    {
        string search = speciality.ToLower();
        int matchCount = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Speciality.ToLower().Contains(search))
            {
                matchCount++;
            }
        }

        Doctor[] result = new Doctor[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Speciality.ToLower().Contains(search))
            {
                result[index++] = _doctors[i];
            }
        }
        return result;
    }

    public Doctor[] GetAll()
    {
        Doctor[] copy = new Doctor[_count];
        Array.Copy(_doctors, copy, _count);
        return copy;
    }

    public bool Remove(int id)
    {
        int indexToRemove = -1;
        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Id == id)
            {
                indexToRemove = i;
                break;
            }
        }

        if (indexToRemove == -1) return false;

        for (int i = indexToRemove; i < _count - 1; i++)
        {
            _doctors[i] = _doctors[i + 1];
        }
        _doctors[_count - 1] = null!;
        _count--;
        return true;
    }

    public void DisplayAll()
    {
        Console.WriteLine($"\n=== Doctors ({_count} / {MaxDoctors}) ===");
        if (_count == 0)
        {
            Console.WriteLine("No doctors.");
            return;
        }
        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine(_doctors[i].ToString());
        }
        Console.WriteLine(new string('-', 60));
    }

    public void DisplayStats()
    {
        Console.WriteLine("\n=== Doctor Statistics ===");
        if (_count == 0)
        {
            Console.WriteLine("No doctors available for statistics.");
            Console.WriteLine(new string('=', 26));
            return;
        }

        int availableNow = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].IsAvailableNow) availableNow++;
        }

        Console.WriteLine($"Total:         {_count}");
        Console.WriteLine($"Available now: {availableNow}");
        Console.WriteLine("By speciality:");

        for (int i = 0; i < _count; i++)
        {
            bool isUnique = true;
            for (int j = 0; j < i; j++)
            {
                if (_doctors[i].Speciality.ToLower() == _doctors[j].Speciality.ToLower())
                {
                    isUnique = false;
                    break;
                }
            }

            if (isUnique)
            {
                int specCount = 0;
                for (int k = 0; k < _count; k++)
                {
                    if (_doctors[k].Speciality.ToLower() == _doctors[i].Speciality.ToLower())
                    {
                        specCount++;
                    }
                }
                Console.WriteLine($"  {_doctors[i].Speciality}: {specCount}");
            }
        }
        Console.WriteLine(new string('=', 26));
    }
}