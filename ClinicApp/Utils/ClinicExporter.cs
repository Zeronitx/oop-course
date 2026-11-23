using System;
using System.IO;
using System.Text;
using ClinicApp.Models;
using ClinicApp.Interfaces;

namespace ClinicApp.Utils;

public class ClinicExporter
{
    private readonly Clinic _clinic;
    private readonly string _baseDir;

    public ClinicExporter(Clinic clinic, string baseDir = "reports")
    {
        _clinic = clinic;
        _baseDir = baseDir;
    }

    private string GetReportDirectory()
    {
        string dir = Path.Combine(_baseDir, DateTime.Today.ToString("yyyy-MM-dd"));
        Directory.CreateDirectory(dir);
        return dir;
    }

    public string ExportPatients()
    {
        string path = Path.Combine(GetReportDirectory(), "patients.txt");
        Patient[] patients = _clinic.Patients.GetAll();

        using StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);
        writer.WriteLine("=== Пацієнти — звіт ===");
        writer.WriteLine($"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm}");
        writer.WriteLine(new string('-', 40));

        for (int i = 0; i < patients.Length; i++)
        {
            writer.WriteLine($"{i + 1,3}. {patients[i]}");
        }

        writer.WriteLine(new string('-', 40));
        writer.WriteLine($"Всього: {patients.Length}");
        return path;
    }

    public string ExportAppointments()
    {
        string path = Path.Combine(GetReportDirectory(), "appointments.txt");
        Appointment[] appointments = _clinic.Appointments.GetAll();

        using StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);
        writer.WriteLine("=== Записи — звіт ===");
        writer.WriteLine($"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm}");
        writer.WriteLine(new string('-', 40));

        for (int i = 0; i < appointments.Length; i++)
        {
            writer.WriteLine($"{i + 1,3}. {appointments[i]}");
        }

        writer.WriteLine(new string('-', 40));
        writer.WriteLine($"Всього: {appointments.Length}");
        return path;
    }

    public string ExportBilling()
    {
        string path = Path.Combine(GetReportDirectory(), "billing.txt");
        IPayable[] unpaid = _clinic.Billing.GetAllUnpaid();

        using StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);
        writer.WriteLine("=== Рахунки (неоплачені) — звіт ===");
        writer.WriteLine($"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm}");
        writer.WriteLine(new string('-', 40));

        for (int i = 0; i < unpaid.Length; i++)
        {
            writer.WriteLine($"{i + 1,3}. {unpaid[i]}");
        }

        writer.WriteLine(new string('-', 40));
        writer.WriteLine($"Всього: {unpaid.Length}");
        writer.WriteLine($"Загальний борг: {_clinic.Billing.GetTotalDebt():F2} грн");
        return path;
    }

    public string ExportTreatmentPlans()
    {
        string path = Path.Combine(GetReportDirectory(), "treatment_plans.txt");
        TreatmentPlan[] plans = _clinic.TreatmentPlans.GetAll();

        using StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);
        writer.WriteLine("=== Плани лікування — звіт ===");
        writer.WriteLine($"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm}");
        writer.WriteLine(new string('-', 40));

        for (int i = 0; i < plans.Length; i++)
        {
            writer.WriteLine($"{i + 1,3}. {plans[i]}");
        }

        writer.WriteLine(new string('-', 40));
        writer.WriteLine($"Всього: {plans.Length}");
        return path;
    }

    public string[] ExportAll()
    {
        return new string[]
        {
            ExportPatients(),
            ExportAppointments(),
            ExportBilling(),
            ExportTreatmentPlans()
        };
    }
}