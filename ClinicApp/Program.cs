using System;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using ClinicApp.Models;
using ClinicApp.Managers;
using ClinicApp.Enums;
using ClinicApp.Interfaces;
using ClinicApp.Comparators;
using ClinicApp.Utils;

namespace ClinicApp;

class Program
{
    static void Main()
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        Console.OutputEncoding = Encoding.UTF8;

        Clinic clinic = new Clinic("Medical Clinic");

        clinic.Patients.Add(new Patient("Ivan", "Petrenko", new DateTime(1985, 5, 10), BloodType.APositive, "0501234567"));
        clinic.Patients.Add(new Patient("Olena", "Koval", new DateTime(1993, 8, 22), BloodType.BNegative, "0672345678"));

        clinic.Doctors.Add(new Doctor("Oleh", "Sydorenko", Speciality.Cardiology));
        clinic.Doctors.Add(new Doctor("Natalia", "Moroz", Speciality.Neurology));

        clinic.Appointments.Book(1, 1, DateTime.Today.AddDays(1).AddHours(10), 30);
        clinic.Appointments.BookUrgent(1, 1, DateTime.Today.AddDays(1).AddHours(11), "біль у грудях", 30);
        clinic.Appointments.BookSpecialist(2, 2, DateTime.Today.AddDays(2).AddHours(9), "кардіологія", 45);

        clinic.MedicalRecords.Add(new Diagnosis(1, 1, DateTime.Today.AddDays(-30), "I10", "Гіпертонічна хвороба", true));
        clinic.MedicalRecords.Add(new LabResult(1, 1, DateTime.Today, "Гемоглобін", 145, "г/л", "120–160", true));

        while (true)
        {
            Console.WriteLine("\n╔══════════════════════════════════════════════════╗");
            Console.WriteLine("║               МЕДИЧНА КЛІНІКА                    ║");
            Console.WriteLine("╠══════════════════════════════════════════════════╣");
            Console.WriteLine("║  1. Пацієнти        — реєстрація, пошук          ║");
            Console.WriteLine("║  2. Лікарі          — персонал, розклад          ║");
            Console.WriteLine("║  3. Записи          — прийоми, скасування        ║");
            Console.WriteLine("║  4. Медична картка  — діагнози, рецепти          ║");
            Console.WriteLine("║  5. Рахунки         — оплата, борги              ║");
            Console.WriteLine("║  6. Черга           — очікування, прийом         ║");
            Console.WriteLine("║  7. Звіт            — загальна статистика        ║");
            Console.WriteLine("║  8. Аналітика       — рейтинги, витрати          ║");
            Console.WriteLine("║  9. Плани лікування — рефлексія, атрибути        ║");
            Console.WriteLine("║  0. Вийти                                        ║");
            Console.WriteLine("╚══════════════════════════════════════════════════╝");
            Console.Write("Оберіть: ");

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    clinic.Patients.DisplayAll();
                    break;
                case "2":
                    DoctorsMenu(clinic);
                    break;
                case "3":
                    AppointmentsMenu(clinic);
                    break;
                case "4":
                    MedicalRecordsMenu(clinic);
                    break;
                case "5":
                    BillingMenu(clinic);
                    break;
                case "6":
                    WaitingRoomMenu(clinic);
                    break;
                case "7":
                    clinic.Patients.DisplayStats();
                    clinic.Doctors.DisplayStats();
                    break;
                case "8":
                    AnalyticsMenu(clinic);
                    break;
                case "9":
                    TreatmentPlansMenu(clinic);
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Невідома команда.");
                    break;
            }
        }
    }

    static void TreatmentPlansMenu(Clinic clinic)
    {
        while (true)
        {
            Console.WriteLine("\n── Плани лікування ────────────────────");
            Console.WriteLine("  1. Показати всі плани");
            Console.WriteLine("  2. Додати план лікування");
            Console.WriteLine("  3. Плани пацієнта");
            Console.WriteLine("  4. Активувати план");
            Console.WriteLine("  5. Завершити план");
            Console.WriteLine("  6. Скасувати план");
            Console.WriteLine("  7. Інформація про тип TreatmentPlan");
            Console.WriteLine("  0. Назад");
            Console.Write("Оберіть: ");

            string choice = Console.ReadLine() ?? "";
            if (choice == "0") return;

            if (choice == "1")
            {
                TreatmentPlan[] plans = clinic.TreatmentPlans.GetAll();
                if (plans.Length == 0)
                {
                    Console.WriteLine("Планів лікування немає.");
                }
                else
                {
                    for (int i = 0; i < plans.Length; i++) Console.WriteLine(plans[i]);
                }
            }
            else if (choice == "2")
            {
                TreatmentPlan plan = FormBuilder.Build<TreatmentPlan>();
                if (clinic.TreatmentPlans.Add(plan))
                {
                    Console.WriteLine($"План лікування [{plan.Id}] додано.");
                }
            }
            else if (choice == "3")
            {
                Console.Write("ID пацієнта: ");
                if (!int.TryParse(Console.ReadLine(), out int pid))
                {
                    Console.WriteLine("Помилка: некоректне число.");
                    continue;
                }

                TreatmentPlan[] plans = clinic.TreatmentPlans.GetByPatient(pid);
                if (plans.Length == 0)
                {
                    Console.WriteLine("Планів для цього пацієнта не знайдено.");
                }
                else
                {
                    for (int i = 0; i < plans.Length; i++) Console.WriteLine(plans[i]);
                }
            }
            else if (choice == "4" || choice == "5" || choice == "6")
            {
                Console.Write("ID плану: ");
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("Помилка: некоректне число.");
                    continue;
                }

                TreatmentPlan? plan = clinic.TreatmentPlans.GetById(id);
                if (plan == null)
                {
                    Console.WriteLine("План не знайдено.");
                    continue;
                }

                bool ok = false;
                if (choice == "4") ok = plan.Activate();
                else if (choice == "5") ok = plan.Complete();
                else if (choice == "6") ok = plan.Cancel();

                if (ok)
                {
                    Console.WriteLine($"Статус плану [{plan.Id}] змінено на {plan.Status}.");
                }
                else
                {
                    Console.WriteLine($"Неможливо змінити статус із {plan.Status}.");
                }
            }
            else if (choice == "7")
            {
                ModelValidator.PrintInfo(typeof(TreatmentPlan));
            }
        }
    }

    static void AnalyticsMenu(Clinic clinic)
    {
        while (true)
        {
            Console.WriteLine("\n── Аналітика ───────────────────");
            Console.WriteLine("  1. Лікарі за навантаженням");
            Console.WriteLine("  2. Лікарі за виручкою");
            Console.WriteLine("  3. Лікарі за іменем");
            Console.WriteLine("  4. Пацієнти за кількістю візитів");
            Console.WriteLine("  5. Пацієнти за витратами");
            Console.WriteLine("  0. Назад");
            Console.Write("Оберіть: ");

            string choice = Console.ReadLine() ?? "";
            if (choice == "0") return;

            if (choice == "1")
            {
                List<DoctorStats> list = CollectDoctorStats(clinic);
                list.Sort();
                for (int i = 0; i < list.Count; i++) Console.WriteLine(list[i]);
            }
            else if (choice == "2")
            {
                List<DoctorStats> list = CollectDoctorStats(clinic);
                list.Sort(new DoctorStatsByRevenue());
                for (int i = 0; i < list.Count; i++) Console.WriteLine(list[i]);
            }
            else if (choice == "3")
            {
                List<DoctorStats> list = CollectDoctorStats(clinic);
                list.Sort(new DoctorStatsByName());
                for (int i = 0; i < list.Count; i++) Console.WriteLine(list[i]);
            }
            else if (choice == "4")
            {
                List<PatientStats> list = CollectPatientStats(clinic);
                list.Sort();
                for (int i = 0; i < list.Count; i++) Console.WriteLine(list[i]);
            }
            else if (choice == "5")
            {
                List<PatientStats> list = CollectPatientStats(clinic);
                list.Sort(new PatientStatsBySpent());
                for (int i = 0; i < list.Count; i++) Console.WriteLine(list[i]);
            }
        }
    }

    static List<DoctorStats> CollectDoctorStats(Clinic clinic)
    {
        List<DoctorStats> list = new List<DoctorStats>();
        foreach (DoctorStats stat in clinic.Analytics.ComputeDoctorStats())
        {
            list.Add(stat);
        }
        return list;
    }

    static List<PatientStats> CollectPatientStats(Clinic clinic)
    {
        List<PatientStats> list = new List<PatientStats>();
        foreach (PatientStats stat in clinic.Analytics.ComputePatientStats())
        {
            list.Add(stat);
        }
        return list;
    }

    static void WaitingRoomMenu(Clinic clinic)
    {
        while (true)
        {
            Console.WriteLine("\n── Черга ───────────────────────");
            Console.WriteLine("  1. Додати пацієнта до черги");
            Console.WriteLine("  2. Прийняти першого");
            Console.WriteLine("  3. Хто перший?");
            Console.WriteLine("  4. Переглянути всю чергу");
            Console.WriteLine("  0. Назад");
            Console.Write("Оберіть: ");

            string choice = Console.ReadLine() ?? "";
            if (choice == "0") return;

            if (choice == "1")
            {
                Console.Write("ID пацієнта: ");
                if (!int.TryParse(Console.ReadLine(), out int pid))
                {
                    Console.WriteLine("Помилка: некоректне число.");
                    continue;
                }

                Patient? p = clinic.Patients.FindById(pid);
                if (p == null)
                {
                    Console.WriteLine("Пацієнта не знайдено.");
                }
                else
                {
                    clinic.WaitingRoom.Enqueue(p);
                    Console.WriteLine($"Пацієнта {p.FullName} додано до черги.");
                }
            }
            else if (choice == "2")
            {
                try
                {
                    Patient p = clinic.WaitingRoom.Dequeue();
                    Console.WriteLine($"Прийнято: {p.FullName}. У черзі лишилось: {clinic.WaitingRoom.Count}");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else if (choice == "3")
            {
                try
                {
                    Patient p = clinic.WaitingRoom.Peek();
                    Console.WriteLine($"Перший у черзі: {p.FullName}");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else if (choice == "4")
            {
                Patient[] queue = clinic.WaitingRoom.ToArray();
                if (queue.Length == 0)
                {
                    Console.WriteLine("Черга порожня.");
                }
                else
                {
                    for (int i = 0; i < queue.Length; i++)
                    {
                        Console.WriteLine($"[{i + 1}] {queue[i].FullName}");
                    }
                }
            }
        }
    }

    static void DoctorsMenu(Clinic clinic)
    {
        while (true)
        {
            Console.WriteLine("\n── Лікарі ────────────────────");
            Console.WriteLine("1. Список лікарів");
            Console.WriteLine("5. Вільні години лікаря");
            Console.WriteLine("0. Назад");
            Console.Write("Оберіть: ");

            string choice = Console.ReadLine() ?? "";
            if (choice == "0") return;

            if (choice == "1")
            {
                clinic.Doctors.DisplayAll();
            }
            else if (choice == "5")
            {
                Console.Write("ID лікаря: ");
                if (!int.TryParse(Console.ReadLine(), out int did))
                {
                    Console.WriteLine("Помилка: некоректне число.");
                    continue;
                }

                Doctor? doc = clinic.Doctors.FindById(did);
                if (doc == null)
                {
                    Console.WriteLine("Лікаря не знайдено.");
                    continue;
                }

                Console.Write("Дата (dd.MM.yyyy): ");
                if (!DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                {
                    Console.WriteLine("Помилка: некоректний формат дати.");
                    continue;
                }

                Console.Write("Скільки слотів: ");
                if (!int.TryParse(Console.ReadLine(), out int slots))
                {
                    Console.WriteLine("Помилка: некоректне число.");
                    continue;
                }

                ISchedulable s = doc;
                try
                {
                    DateTime[] available = s.GetAvailableSlots(date, slots);
                    for (int i = 0; i < available.Length; i++)
                    {
                        Console.WriteLine(available[i].ToString("HH:mm"));
                    }
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine($"Помилка: {ex.Message}");
                }
            }
        }
    }

    static void AppointmentsMenu(Clinic clinic)
    {
        while (true)
        {
            Console.WriteLine("\n── Записи ────────────────────");
            Console.WriteLine("1. Список записів");
            Console.WriteLine("8. Скасувати всі записи пацієнта");
            Console.WriteLine("9. За типом прийому");
            Console.WriteLine("0. Назад");
            Console.Write("Оберіть: ");

            string choice = Console.ReadLine() ?? "";
            if (choice == "0") return;

            if (choice == "1")
            {
                clinic.Appointments.DisplayList(clinic.Appointments.GetUpcoming());
            }
            else if (choice == "8")
            {
                Console.Write("ID пацієнта: ");
                if (!int.TryParse(Console.ReadLine(), out int pid))
                {
                    Console.WriteLine("Помилка: некоректне число.");
                    continue;
                }

                Console.Write("Причина (Enter — без причини): ");
                string reason = Console.ReadLine() ?? "";

                int cancelled = AppointmentManager.CancelAll(clinic.Appointments.GetByPatient(pid), reason);
                Console.WriteLine($"Скасовано записів: {cancelled}");
            }
            else if (choice == "9")
            {
                Console.WriteLine("  1. Термінові");
                Console.WriteLine("  2. Консультації спеціаліста");
                Console.WriteLine("  3. Звичайні");
                Console.Write("Оберіть: ");
                string subChoice = Console.ReadLine() ?? "";

                Appointment[] list = new Appointment[0];
                if (subChoice == "1") list = clinic.Appointments.GetUrgent();
                else if (subChoice == "2") list = clinic.Appointments.GetSpecialist();
                else if (subChoice == "3") list = clinic.Appointments.GetRegular();
                else
                {
                    Console.WriteLine("Невідома команда.");
                    continue;
                }

                clinic.Appointments.DisplayList(list);
            }
        }
    }

    static void BillingMenu(Clinic clinic)
    {
        while (true)
        {
            Console.WriteLine("\n── Рахунки ───────────────────");
            Console.WriteLine("  1. Борги пацієнта");
            Console.WriteLine("  2. Всі неоплачені записи");
            Console.WriteLine("  3. Оплатити запис");
            Console.WriteLine("  4. Загальний борг клініки");
            Console.WriteLine("  0. Назад");
            Console.Write("Оберіть: ");

            string choice = Console.ReadLine() ?? "";
            if (choice == "0") return;

            if (choice == "1")
            {
                Console.Write("ID пацієнта: ");
                if (!int.TryParse(Console.ReadLine(), out int pid))
                {
                    Console.WriteLine("Помилка: некоректне число.");
                    continue;
                }

                IPayable[] unpaid = clinic.Billing.GetUnpaidByPatient(pid);
                Console.WriteLine($"Неоплачені записи пацієнта #{pid}:");
                clinic.Billing.DisplayUnpaid(unpaid);
                Console.WriteLine($"Борг: {clinic.Billing.GetPatientDebt(pid):F2} грн");
            }
            else if (choice == "2")
            {
                clinic.Billing.DisplayUnpaid(clinic.Billing.GetAllUnpaid());
            }
            else if (choice == "3")
            {
                Console.Write("ID запису для оплати: ");
                if (!int.TryParse(Console.ReadLine(), out int aid))
                {
                    Console.WriteLine("Помилка: некоректне число.");
                    continue;
                }

                bool success = clinic.Billing.PayAppointment(aid);
                if (success)
                {
                    Console.WriteLine($"Запис [{aid}] оплачено.");
                }
                else
                {
                    Console.WriteLine("Не вдалося оплатити: запис не знайдено, вже оплачено або скасовано.");
                }
            }
            else if (choice == "4")
            {
                Console.WriteLine($"Загальний борг по клініці: {clinic.Billing.GetTotalDebt():F2} грн");
            }
        }
    }

    static void MedicalRecordsMenu(Clinic clinic)
    {
        while (true)
        {
            Console.WriteLine("\n── Медична картка ────────────");
            Console.WriteLine("1. Картка пацієнта");
            Console.WriteLine("2. Усі записи пацієнта");
            Console.WriteLine("0. Назад");
            Console.Write("Оберіть: ");

            string choice = Console.ReadLine() ?? "";
            if (choice == "0") return;

            if (choice == "1" || choice == "2")
            {
                Console.Write("ID пацієнта: ");
                if (!int.TryParse(Console.ReadLine(), out int pid)) { Console.WriteLine("Помилка: некоректне число."); continue; }

                if (choice == "1") clinic.MedicalRecords.DisplayPatientSummary(pid);
                else clinic.MedicalRecords.DisplayList(clinic.MedicalRecords.GetByPatient(pid));
            }
        }
    }
}