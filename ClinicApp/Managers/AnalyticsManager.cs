using System;
using System.Collections.Generic;
using System.Linq;
using ClinicApp.Models;

namespace ClinicApp.Managers;

public class AnalyticsManager
{
    private AppointmentManager _appointments;
    private DoctorManager _doctors;
    private PatientManager _patients;

    public AnalyticsManager(AppointmentManager appointments, DoctorManager doctors, PatientManager patients)
    {
        _appointments = appointments;
        _doctors = doctors;
        _patients = patients;
    }

    public IEnumerable<DoctorStats> ComputeDoctorStats()
    {
        Appointment[] allAppointments = _appointments.GetAll();
        return _doctors.GetAll().Select(d =>
        {
            var docAppts = allAppointments.Where(a => a.DoctorId == d.Id);
            return new DoctorStats(
                d.Id,
                d.FullName,
                docAppts.Count(),
                docAppts.Sum(a => a.GetCost()),
                docAppts.Any() ? docAppts.Max(a => a.ScheduledAt) : DateTime.MinValue);
        });
    }

    public IEnumerable<PatientStats> ComputePatientStats()
    {
        Appointment[] allAppointments = _appointments.GetAll();
        return _patients.GetAll().Select(p =>
        {
            var patAppts = allAppointments.Where(a => a.PatientId == p.Id);
            return new PatientStats(
                p.Id,
                p.FullName,
                patAppts.Count(),
                patAppts.Sum(a => a.GetCost()),
                patAppts.Any() ? patAppts.Max(a => a.ScheduledAt) : DateTime.MinValue);
        });
    }
}