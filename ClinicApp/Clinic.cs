using System;
using ClinicApp.Models;
using ClinicApp.Managers;
using ClinicApp.Utils;

namespace ClinicApp;

public class Clinic
{
    public string Name { get; }
    public PatientManager Patients { get; }
    public DoctorManager Doctors { get; }
    public AppointmentManager Appointments { get; }
    public MedicalRecordManager MedicalRecords { get; }
    public BillingManager Billing { get; }
    public WaitingQueue<Patient> WaitingRoom { get; }
    public AnalyticsManager Analytics { get; }
    public TreatmentPlanManager TreatmentPlans { get; }
    public ClinicLogger Logger { get; }
    public ClinicExporter Exporter { get; }
    public SessionManager Session { get; }

    public Clinic(string name)
    {
        Name = name;
        Patients = new PatientManager();
        Doctors = new DoctorManager();
        Appointments = new AppointmentManager(Patients, Doctors);
        MedicalRecords = new MedicalRecordManager();
        Billing = new BillingManager(Appointments);
        WaitingRoom = new WaitingQueue<Patient>();
        Analytics = new AnalyticsManager(Appointments, Doctors, Patients);
        TreatmentPlans = new TreatmentPlanManager();
        Logger = new ClinicLogger();
        Exporter = new ClinicExporter(this);
        Session = new SessionManager();

        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        Patients.PatientAdded += Logger.OnPatientAdded;
        Appointments.AppointmentBooked += Logger.OnAppointmentBooked;
        Appointments.UrgentAppointmentBooked += Logger.OnUrgentAppointmentBooked;
        Appointments.AppointmentCancelled += Logger.OnAppointmentCancelled;
        Appointments.AppointmentCompleted += Logger.OnAppointmentCompleted;
        Billing.PaymentReceived += Logger.OnPaymentReceived;
        TreatmentPlans.PlanCompleted += Logger.OnPlanCompleted;
    }
}