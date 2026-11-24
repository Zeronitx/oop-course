using System;
using System.Collections.Generic;
using System.Linq;
using ClinicApp.Models;
using ClinicApp.Enums;

namespace ClinicApp.Managers;

public class ReportManager
{
    private readonly AppointmentManager _appointments;
    private readonly DoctorManager _doctors;
    private readonly PatientManager _patients;

    public ReportManager(AppointmentManager appointments, DoctorManager doctors, PatientManager patients)
    {
        _appointments = appointments;
        _doctors = doctors;
        _patients = patients;
    }

    public IEnumerable<SpecialityReport> GetSpecialityStats()
    {
        Appointment[] allAppointments = _appointments.GetAll();
        return _doctors.GetAll()
            .GroupBy(d => d.Speciality)
            .Select(g =>
            {
                int[] doctorIds = g.Select(d => d.Id).ToArray();
                var specAppts = allAppointments.Where(a => doctorIds.Contains(a.DoctorId));
                return new SpecialityReport(
                    g.Key,
                    g.Count(),
                    specAppts.Count(),
                    specAppts.Sum(a => a.GetCost()));
            })
            .OrderByDescending(r => r.TotalRevenue);
    }

    public string? FindBusiestDoctorName()
    {
        Appointment[] allAppointments = _appointments.GetAll();
        return _doctors.GetAll()
            .OrderByDescending(d => allAppointments.Count(a => a.DoctorId == d.Id))
            .Select(d => d.FullName)
            .FirstOrDefault();
    }

    public IEnumerable<string> GetPatientsWithMultipleVisits(int minVisits)
    {
        return _appointments.GetAll()
            .GroupBy(a => a.PatientId)
            .Where(g => g.Count() >= minVisits)
            .Join(
                _patients.GetAll(),
                g => g.Key,
                p => p.Id,
                (g, p) => p.FullName);
    }

    public IEnumerable<DoctorStats> GetTopEarners(int n)
    {
        AnalyticsManager analytics = new AnalyticsManager(_appointments, _doctors, _patients);
        return analytics.ComputeDoctorStats()
            .OrderByDescending(s => s.TotalRevenue)
            .Take(n);
    }

    public bool HasAnyUrgentAppointments()
    {
        return _appointments.GetAll().Any(a => a is UrgentAppointment);
    }

    public IEnumerable<Speciality> GetActiveSpecialities()
    {
        return _doctors.GetAll()
            .Select(d => d.Speciality)
            .Distinct()
            .OrderBy(s => s);
    }

    public IEnumerable<(int Year, int Month, decimal Total)> GetMonthlyRevenue()
    {
        return _appointments.GetAll()
            .GroupBy(a => new { a.ScheduledAt.Year, a.ScheduledAt.Month })
            .OrderBy(g => g.Key.Year)
            .ThenBy(g => g.Key.Month)
            .Select(g => (g.Key.Year, g.Key.Month, g.Sum(a => a.GetCost())));
    }
}