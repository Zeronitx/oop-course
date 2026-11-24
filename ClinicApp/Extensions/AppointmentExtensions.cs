using System;
using System.Collections.Generic;
using System.Linq;
using ClinicApp.Models;

namespace ClinicApp.Extensions;

public static class AppointmentExtensions
{
    public static IEnumerable<Appointment> Unpaid(this IEnumerable<Appointment> source) =>
        source.Where(a => !a.IsPaid && !a.IsCancelled);

    public static IEnumerable<Appointment> Upcoming(this IEnumerable<Appointment> source) =>
        source.Where(a => a.IsUpcoming);

    public static IEnumerable<Appointment> ByDoctor(this IEnumerable<Appointment> source, int doctorId) =>
        source.Where(a => a.DoctorId == doctorId);

    public static IEnumerable<Appointment> ByPatient(this IEnumerable<Appointment> source, int patientId) =>
        source.Where(a => a.PatientId == patientId);

    public static IEnumerable<Appointment> Overdue(this IEnumerable<Appointment> source) =>
        source.Where(a => a.ScheduledAt < DateTime.Now && !a.IsCancelled && !a.IsPaid);

    public static IEnumerable<Appointment> CostAbove(this IEnumerable<Appointment> source, decimal minCost) =>
        source.Where(a => a.GetCost() > minCost);

    public static decimal TotalCost(this IEnumerable<Appointment> source) =>
        source.Sum(a => a.GetCost());
}