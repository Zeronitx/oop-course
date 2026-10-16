using System;
using ClinicApp.Models;

namespace ClinicApp.Managers;

public class MedicalRecordManager
{
    private const int MaxRecords = 1000;
    private MedicalRecord[] _records = new MedicalRecord[MaxRecords];
    private int _count = 0;

    public int Count => _count;

    public MedicalRecord? this[int index]
    {
        get
        {
            if (index >= 0 && index < _count) return _records[index];
            return null;
        }
    }

    public void Add(MedicalRecord record)
    {
        if (_count >= MaxRecords)
        {
            Console.WriteLine("Помилка: досягнуто ліміт медичних записів.");
            return;
        }
        _records[_count++] = record;
        Console.WriteLine($"Запис [{record.Id}] {record.GetRecordType()} додано.");
    }

    public MedicalRecord? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].Id == id) return _records[i];
        }
        return null;
    }

    public MedicalRecord[] GetByPatient(int patientId)
    {
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId) matchCount++;
        }

        MedicalRecord[] result = new MedicalRecord[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId) result[index++] = _records[i];
        }
        return result;
    }

    public MedicalRecord[] GetByDoctor(int doctorId)
    {
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].DoctorId == doctorId) matchCount++;
        }

        MedicalRecord[] result = new MedicalRecord[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].DoctorId == doctorId) result[index++] = _records[i];
        }
        return result;
    }

    public Diagnosis[] GetDiagnoses(int patientId)
    {
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Diagnosis) matchCount++;
        }

        Diagnosis[] result = new Diagnosis[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Diagnosis d) result[index++] = d;
        }
        return result;
    }

    public LabResult[] GetLabResults(int patientId)
    {
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is LabResult) matchCount++;
        }

        LabResult[] result = new LabResult[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is LabResult l) result[index++] = l;
        }
        return result;
    }

    public Prescription[] GetPrescriptions(int patientId)
    {
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Prescription) matchCount++;
        }

        Prescription[] result = new Prescription[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Prescription p) result[index++] = p;
        }
        return result;
    }

    public Diagnosis[] GetChronicDiagnoses(int patientId)
    {
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Diagnosis d && d.IsChronic) matchCount++;
        }

        Diagnosis[] result = new Diagnosis[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Diagnosis d && d.IsChronic) result[index++] = d;
        }
        return result;
    }

    public Prescription[] GetActivePrescriptions(int patientId)
    {
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Prescription p && p.IsActive()) matchCount++;
        }

        Prescription[] result = new Prescription[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Prescription p && p.IsActive()) result[index++] = p;
        }
        return result;
    }

    public void DisplayPatientSummary(int patientId)
    {
        Diagnosis[] diagnoses = GetDiagnoses(patientId);
        LabResult[] labs = GetLabResults(patientId);
        Prescription[] prescriptions = GetPrescriptions(patientId);

        int total = diagnoses.Length + labs.Length + prescriptions.Length;

        Console.WriteLine($"\n=== Медична картка пацієнта #{patientId} ===");
        if (total == 0)
        {
            Console.WriteLine("Немає записів.");
            return;
        }

        Console.WriteLine($"Всього записів: {total} (діагнозів: {diagnoses.Length}, аналізів: {labs.Length}, рецептів: {prescriptions.Length})");

        Diagnosis[] chronic = GetChronicDiagnoses(patientId);
        if (chronic.Length > 0)
        {
            Console.WriteLine($"Хронічні діагнози ({chronic.Length}):");
            for (int i = 0; i < chronic.Length; i++)
            {
                Console.WriteLine($"  {chronic[i]}");
            }
        }

        Prescription[] active = GetActivePrescriptions(patientId);
        if (active.Length > 0)
        {
            Console.WriteLine($"Активні рецепти ({active.Length}):");
            for (int i = 0; i < active.Length; i++)
            {
                Console.WriteLine($"  {active[i]} | до {active[i].ExpiresAt:dd.MM.yyyy}");
            }
        }
    }

    public void DisplayList(MedicalRecord[] list)
    {
        if (list.Length == 0)
        {
            Console.WriteLine("Записів не знайдено.");
            return;
        }
        for (int i = 0; i < list.Length; i++)
        {
            Console.WriteLine(list[i].ToString());
        }
    }

    public void DisplayAll()
    {
        Console.WriteLine($"\n=== Усі медичні записи ({_count} / {MaxRecords}) ===");
        if (_count == 0)
        {
            Console.WriteLine("Записів немає.");
            return;
        }
        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine(_records[i].ToString());
        }
        Console.WriteLine(new string('-', 60));
    }
}