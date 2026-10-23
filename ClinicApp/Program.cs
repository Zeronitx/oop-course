using System;
using System.Globalization;
using System.Text;
using System.Threading;
using ClinicApp.Models;
using ClinicApp.Managers;
using ClinicApp.Enums;
using ClinicApp.Interfaces;

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
        clinic.Appointments.Book(1, 1, DateTime.Today.AddDays(1).AddHours(11), 45);
        clinic.Appointments.Book(2, 2, DateTime.Today.AddDays(2).AddHours(9), 20);

        clinic.MedicalRecords.Add(new Diagnosis(1, 1, DateTime.Today.AddDays(-30), "I10", "Гіпертонічна хвороба", true));
        clinic.MedicalRecords.Add(new LabResult(1, 1, DateTime.Today, "Гемоглобін", 145, "г/л", "120–160", true));

        while (true)
        {
            Console.WriteLine("\n╔══════════════════════════════════════════════╗");
            Console.WriteLine("║           МЕДИЧНА КЛІНІКА                    ║");
            Console.WriteLine("╠══════════════════════════════════════════════╣");
            Console.WriteLine("║  1. Пацієнти       — реєстрація, пошук       ║");
            Console.WriteLine("║  2. Лікарі         — персонал, розклад       ║");
            Console.WriteLine("║  3. Записи         — прийоми, скасування     ║");
            Console.WriteLine("║  4. Медична картка — діагнози, рецепти       ║");
            Console.WriteLine("║  5. Рахунки        — оплата, борги           ║");
            Console.WriteLine("║  6. Звіт           — загальна статистика     ║");
            Console.WriteLine("║  0. Вийти                                    ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝");
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
                    clinic.Patients.DisplayStats();
                    clinic.Doctors.DisplayStats();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Невідома команда.");
                    break;
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