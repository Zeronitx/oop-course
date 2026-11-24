using System.Collections.Generic;
using System.Linq;
using ClinicApp.Models;
using ClinicApp.Enums;

namespace ClinicApp.Extensions;

public static class PatientExtensions
{
    public static IEnumerable<Patient> Adults(this IEnumerable<Patient> source) =>
        source.Where(p => p.Age >= 18);

    public static IEnumerable<Patient> ByBloodType(this IEnumerable<Patient> source, BloodType bloodType) =>
        source.Where(p => p.BloodType == bloodType);

    public static IEnumerable<Patient> WithAppointments(this IEnumerable<Patient> source, IEnumerable<Appointment> appointments) =>
        source.Where(p => appointments.Any(a => a.PatientId == p.Id));
}