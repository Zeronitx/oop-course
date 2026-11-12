using System;
using System.Collections.Generic;
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
        Doctor[] allDoctors = _doctors.GetAll();
        Appointment[] allAppointments = _appointments.GetAll();

        for (int i = 0; i < allDoctors.Length; i++)
        {
            Doctor doc = allDoctors[i];
            int count = 0;
            decimal revenue = 0m;
            DateTime lastDate = DateTime.MinValue;

            for (int j = 0; j < allAppointments.Length; j++)
            {
                Appointment appt = allAppointments[j];
                if (appt.DoctorId == doc.Id)
                {
                    count++;
                    revenue += appt.GetCost();
                    if (appt.ScheduledAt > lastDate)
                    {
                        lastDate = appt.ScheduledAt;
                    }
                }
            }

            yield return new DoctorStats(doc.Id, doc.FullName, count, revenue, lastDate);
        }
    }

    public IEnumerable<PatientStats> ComputePatientStats()
    {
        Patient[] allPatients = _patients.GetAll();
        Appointment[] allAppointments = _appointments.GetAll();

        for (int i = 0; i < allPatients.Length; i++)
        {
            Patient pat = allPatients[i];
            int count = 0;
            decimal spent = 0m;
            DateTime lastDate = DateTime.MinValue;

            for (int j = 0; j < allAppointments.Length; j++)
            {
                Appointment appt = allAppointments[j];
                if (appt.PatientId == pat.Id)
                {
                    count++;
                    spent += appt.GetCost();
                    if (appt.ScheduledAt > lastDate)
                    {
                        lastDate = appt.ScheduledAt;
                    }
                }
            }

            yield return new PatientStats(pat.Id, pat.FullName, count, spent, lastDate);
        }
    }
}