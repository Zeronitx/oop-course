using System;
using ClinicApp.Enums;
using ClinicApp.Utils;
using ClinicApp.Interfaces;

namespace ClinicApp.Models;

public class Appointment : IPayable, ICancellable
{
    private static int _nextId = 1;
    private int _durationMinutes;

    public int Id { get; }
    public int PatientId { get; }
    public int DoctorId { get; }
    public DateTime ScheduledAt { get; set; }

    public int DurationMinutes
    {
        get => _durationMinutes;
        set
        {
            ClinicValidator.ValidatePositive(value, nameof(DurationMinutes));
            _durationMinutes = value;
        }
    }

    public AppointmentStatus Status { get; private set; }
    public string Notes { get; private set; }
    public bool IsPaid { get; private set; }

    public DateTime EndsAt => ScheduledAt.AddMinutes(DurationMinutes);
    public bool IsUpcoming => ScheduledAt > DateTime.Now && Status == AppointmentStatus.Scheduled;

    public bool IsCancelled => Status == AppointmentStatus.Cancelled;
    public string CancellationReason => IsCancelled ? Notes : "";

    // Варіант А: Множник знижки в базовому класі.
    // Обрано цей варіант, бо він дозволяє додавати знижку (наприклад, 0.8 для VIP) 
    // до будь-якого типу прийому без зміни конструкторів підкласів. 
    // Оскільки підкласи використовують base.GetCost(), знижка застосується автоматично.
    public decimal DiscountMultiplier { get; set; } = 1.0m;

    public Appointment(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30)
    {
        PatientId = patientId;
        DoctorId = doctorId;
        ScheduledAt = scheduledAt;
        DurationMinutes = durationMinutes;
        Status = AppointmentStatus.Scheduled;
        Notes = "";
        IsPaid = false;

        Id = _nextId++;
    }

    public virtual decimal GetCost()
    {
        return DurationMinutes * 10m * DiscountMultiplier;
    }

    public virtual string GetDescription()
    {
        return "Прийом";
    }

    public int GetPriority()
    {
        return 3;
    }

    public void MarkPaid()
    {
        if (!IsCancelled)
        {
            IsPaid = true;
        }
    }

    public bool Cancel(string reason = "")
    {
        if (Status != AppointmentStatus.Scheduled) return false;

        Status = AppointmentStatus.Cancelled;
        if (reason.Length > 0)
        {
            Notes = reason;
        }
        return true;
    }

    public bool Complete()
    {
        if (Status != AppointmentStatus.Scheduled) return false;

        Status = AppointmentStatus.Completed;
        return true;
    }

    public override string ToString()
    {
        string baseInfo = $"[{Id}] {GetDescription()} | Patient #{PatientId} -> Doctor #{DoctorId} | {ScheduledAt:dd.MM.yyyy HH:mm}-{EndsAt:HH:mm} | {Status} | {GetCost():F2} грн";
        if (Notes.Length > 0)
        {
            return $"{baseInfo} | {Notes}";
        }
        return baseInfo;
    }
}