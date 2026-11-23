using System;
using System.IO;
using System.Text;
using ClinicApp.Models;
using ClinicApp.Events;

namespace ClinicApp.Utils;

public class SessionEventTracker
{
    private readonly Clinic _clinic;

    public int PatientsAdded { get; private set; }
    public int AppointmentsBooked { get; private set; }
    public int UrgentBooked { get; private set; }
    public int AppointmentsCancelled { get; private set; }
    public int AppointmentsCompleted { get; private set; }
    public int PaymentsReceived { get; private set; }
    public int PlansCompleted { get; private set; }

    public SessionEventTracker(Clinic clinic)
    {
        _clinic = clinic;
    }

    public void OnPatientAdded(object? sender, PatientEventArgs e) => PatientsAdded++;

    public void OnAppointmentBooked(object? sender, AppointmentEventArgs e) => AppointmentsBooked++;

    public void OnUrgentAppointmentBooked(object? sender, AppointmentEventArgs e) => UrgentBooked++;

    public void OnAppointmentCancelled(object? sender, AppointmentEventArgs e)
    {
        AppointmentsCancelled++;
        if (_clinic.WaitingRoom.Count > 0)
        {
            Patient next = _clinic.WaitingRoom.Peek();
            Console.WriteLine($"  [ЧЕРГА] Слот звільнився. Наступний: {next.FullName}");
        }
    }

    public void OnAppointmentCompleted(object? sender, AppointmentEventArgs e) => AppointmentsCompleted++;

    public void OnPaymentReceived(object? sender, PaymentEventArgs e) => PaymentsReceived++;

    public void OnPlanCompleted(object? sender, TreatmentPlanEventArgs e) => PlansCompleted++;

    private string BuildSummaryText()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("=== Підсумок сесії ===");
        sb.AppendLine($"Пацієнтів додано:    {PatientsAdded}");
        sb.AppendLine($"Записів створено:    {AppointmentsBooked} (термінових: {UrgentBooked})");
        sb.AppendLine($"Скасовано:           {AppointmentsCancelled}");
        sb.AppendLine($"Завершено:           {AppointmentsCompleted}");
        sb.AppendLine($"Оплат:               {PaymentsReceived}");
        sb.AppendLine($"Планів завершено:    {PlansCompleted}");
        return sb.ToString();
    }

    public void PrintSummary()
    {
        Console.WriteLine();
        Console.Write(BuildSummaryText());
    }

    public void SaveSummary(string path = "session_summary.txt")
    {
        using StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);
        writer.WriteLine($"Сформовано: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
        writer.Write(BuildSummaryText());
    }
}