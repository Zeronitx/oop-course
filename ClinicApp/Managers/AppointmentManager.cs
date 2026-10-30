using System;
using ClinicApp.Models;
using ClinicApp.Enums;
using ClinicApp.Interfaces;

namespace ClinicApp.Managers;

public class AppointmentManager
{
    private const int MaxAppointments = 500;
    private Appointment[] _appointments = new Appointment[MaxAppointments];
    private int _count = 0;

    private PatientManager _patients;
    private DoctorManager _doctors;

    public int Count => _count;

    public Appointment? this[int index]
    {
        get
        {
            if (index >= 0 && index < _count) return _appointments[index];
            return null;
        }
    }

    public AppointmentManager(PatientManager patients, DoctorManager doctors)
    {
        _patients = patients;
        _doctors = doctors;
    }

    private Appointment? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].Id == id) return _appointments[i];
        }
        return null;
    }

    private bool TryAddAppointment(Appointment appt)
    {
        if (_count >= MaxAppointments)
        {
            Console.WriteLine("Error: Maximum capacity of appointments reached.");
            return false;
        }

        Patient? patient = _patients.FindById(appt.PatientId);
        if (patient == null)
        {
            Console.WriteLine($"Error: Patient with ID {appt.PatientId} not found.");
            return false;
        }

        Doctor? doctor = _doctors.FindById(appt.DoctorId);
        if (doctor == null)
        {
            Console.WriteLine($"Error: Doctor with ID {appt.DoctorId} not found.");
            return false;
        }

        _appointments[_count++] = appt;
        Console.WriteLine($"Appointment [{appt.Id}] booked: {patient.FullName} -> {doctor.FullName} at {appt.ScheduledAt:dd.MM.yyyy HH:mm}");
        return true;
    }

    public bool Book(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30)
    {
        return TryAddAppointment(new RegularAppointment(patientId, doctorId, scheduledAt, durationMinutes));
    }

    public bool BookUrgent(int patientId, int doctorId, DateTime scheduledAt, string urgencyNote = "", int durationMinutes = 30)
    {
        return TryAddAppointment(new UrgentAppointment(patientId, doctorId, scheduledAt, urgencyNote, durationMinutes));
    }

    public bool BookSpecialist(int patientId, int doctorId, DateTime scheduledAt, string topic = "", int durationMinutes = 45)
    {
        return TryAddAppointment(new SpecialistAppointment(patientId, doctorId, scheduledAt, topic, durationMinutes));
    }

    public bool Cancel(int id, string reason = "")
    {
        Appointment? appt = FindById(id);
        if (appt == null) return false;

        bool success = appt.Cancel(reason);
        if (success)
        {
            Console.WriteLine($"Appointment [{id}] cancelled.");
        }
        return success;
    }

    public bool Complete(int id)
    {
        Appointment? appt = FindById(id);
        if (appt == null) return false;

        return appt.Complete();
    }

    public Appointment[] GetByPatient(int patientId)
    {
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].PatientId == patientId) matchCount++;
        }

        Appointment[] result = new Appointment[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].PatientId == patientId) result[index++] = _appointments[i];
        }
        return result;
    }

    public Appointment[] GetByDoctor(int doctorId)
    {
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].DoctorId == doctorId) matchCount++;
        }

        Appointment[] result = new Appointment[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].DoctorId == doctorId) result[index++] = _appointments[i];
        }
        return result;
    }

    public Appointment[] GetByDate(DateTime date)
    {
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].ScheduledAt.Date == date.Date) matchCount++;
        }

        Appointment[] result = new Appointment[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].ScheduledAt.Date == date.Date) result[index++] = _appointments[i];
        }
        return result;
    }

    public Appointment[] GetByDate(int year, int month, int day)
    {
        return GetByDate(new DateTime(year, month, day));
    }

    public Appointment[] GetUpcoming()
    {
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].IsUpcoming) matchCount++;
        }

        Appointment[] result = new Appointment[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].IsUpcoming) result[index++] = _appointments[i];
        }
        return result;
    }

    public Appointment[] GetUrgent()
    {
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i] is UrgentAppointment) matchCount++;
        }

        Appointment[] result = new Appointment[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i] is UrgentAppointment) result[index++] = _appointments[i];
        }
        return result;
    }

    public Appointment[] GetSpecialist()
    {
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i] is SpecialistAppointment) matchCount++;
        }

        Appointment[] result = new Appointment[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i] is SpecialistAppointment) result[index++] = _appointments[i];
        }
        return result;
    }

    public Appointment[] GetRegular()
    {
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i] is RegularAppointment) matchCount++;
        }

        Appointment[] result = new Appointment[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i] is RegularAppointment) result[index++] = _appointments[i];
        }
        return result;
    }

    public void DisplayAppointment(Appointment appt)
    {
        Patient? p = _patients.FindById(appt.PatientId);
        Doctor? d = _doctors.FindById(appt.DoctorId);

        string pName = p != null ? p.FullName : $"Patient #{appt.PatientId}";
        string dName = d != null ? d.FullName : $"Doctor #{appt.DoctorId}";

        string baseInfo = $"[{appt.Id}] {appt.GetDescription()} | {pName} -> {dName} | {appt.ScheduledAt:dd.MM.yyyy HH:mm}-{appt.EndsAt:HH:mm} | {appt.Status} | {appt.GetCost():F2} грн";

        if (appt.Notes.Length > 0)
        {
            Console.WriteLine($"{baseInfo} | {appt.Notes}");
        }
        else
        {
            Console.WriteLine(baseInfo);
        }
    }

    public void DisplayList(Appointment[] list)
    {
        if (list.Length == 0)
        {
            Console.WriteLine("No appointments found.");
            return;
        }
        for (int i = 0; i < list.Length; i++)
        {
            DisplayAppointment(list[i]);
        }
    }

    public Appointment[] GetAll()
    {
        Appointment[] copy = new Appointment[_count];
        for (int i = 0; i < _count; i++)
        {
            copy[i] = _appointments[i];
        }
        return copy;
    }

    public static int CancelAll(ICancellable[] items, string reason = "")
    {
        int count = 0;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].Cancel(reason))
            {
                count++;
            }
        }
        return count;
    }
}