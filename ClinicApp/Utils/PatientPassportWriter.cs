using System;
using System.IO;
using System.Text;
using ClinicApp.Models;
using ClinicApp.Events;

namespace ClinicApp.Utils;

public class PatientPassportWriter
{
    private readonly Clinic _clinic;
    private readonly string _outputDir;

    public PatientPassportWriter(Clinic clinic, string outputDir = "patients")
    {
        _clinic = clinic;
        _outputDir = outputDir;
        Directory.CreateDirectory(_outputDir);
    }

    public void OnPatientAdded(object? sender, PatientEventArgs e) =>
        WritePassport(e.PatientId);

    public void OnAppointmentCompleted(object? sender, AppointmentEventArgs e) =>
        WritePassport(e.PatientId);

    public void OnPlanCompleted(object? sender, TreatmentPlanEventArgs e) =>
        WritePassport(e.PatientId);

    private void WritePassport(int patientId)
    {
        Patient? patient = _clinic.Patients.FindById(patientId);
        if (patient == null) return;

        string path = Path.Combine(_outputDir, $"passport_{patientId}.txt");
        using StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);

        writer.WriteLine($"=== Паспорт пацієнта #{patient.Id} ===");
        writer.WriteLine($"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm}");
        writer.WriteLine();

        writer.WriteLine($"Ім'я: {patient.FullName}");
        writer.WriteLine($"Дата народження: {patient.DateOfBirth:dd.MM.yyyy} ({patient.Age} років)");
        writer.WriteLine($"Група крові: {ClinicFormatter.FormatBloodType(patient.BloodType)}   Телефон: {ClinicFormatter.FormatPhone(patient.Phone)}");
        writer.WriteLine();

        MedicalRecord[] records = _clinic.MedicalRecords.GetByPatient(patientId);

        writer.WriteLine("-- Діагнози --");
        bool hasDiagnoses = false;
        for (int i = 0; i < records.Length; i++)
        {
            if (records[i] is Diagnosis d)
            {
                writer.WriteLine(d);
                hasDiagnoses = true;
            }
        }
        if (!hasDiagnoses) writer.WriteLine("Немає");

        writer.WriteLine("-- Аналізи --");
        bool hasLabResults = false;
        for (int i = 0; i < records.Length; i++)
        {
            if (records[i] is LabResult lr)
            {
                writer.WriteLine(lr);
                hasLabResults = true;
            }
        }
        if (!hasLabResults) writer.WriteLine("Немає");

        writer.WriteLine("-- Рецепти --");
        bool hasPrescriptions = false;
        for (int i = 0; i < records.Length; i++)
        {
            if (records[i] is Prescription pr)
            {
                writer.WriteLine(pr);
                hasPrescriptions = true;
            }
        }
        if (!hasPrescriptions) writer.WriteLine("Немає");
        writer.WriteLine();

        Appointment[] appointments = _clinic.Appointments.GetByPatient(patientId);
        writer.WriteLine("-- Записи --");
        if (appointments.Length == 0)
        {
            writer.WriteLine("Немає");
        }
        else
        {
            for (int i = 0; i < appointments.Length; i++)
            {
                writer.WriteLine(appointments[i]);
            }
        }
        writer.WriteLine();

        TreatmentPlan[] plans = _clinic.TreatmentPlans.GetByPatient(patientId);
        writer.WriteLine("-- Плани лікування --");
        if (plans.Length == 0)
        {
            writer.WriteLine("Немає");
        }
        else
        {
            for (int i = 0; i < plans.Length; i++)
            {
                writer.WriteLine(plans[i]);
            }
        }
        writer.WriteLine();

        writer.WriteLine($"Заборгованість: {_clinic.Billing.GetPatientDebt(patientId):F2} грн");
    }
}