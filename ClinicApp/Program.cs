using System;
using System.Globalization;
using System.Text;
using System.Threading;
using ClinicApp.Models;
using ClinicApp.Managers;
using ClinicApp.Enums;

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
        clinic.Patients.Add(new Patient("Maksym", "Boiko", new DateTime(2010, 1, 15), BloodType.OPositive, "0933456789"));

        clinic.Doctors.Add(new Doctor("Oleh", "Sydorenko", Speciality.Cardiology));
        clinic.Doctors.Add(new Doctor("Natalia", "Moroz", Speciality.Neurology));

        clinic.MedicalRecords.Add(new Diagnosis(1, 1, DateTime.Today.AddDays(-30), "I10", "Гіпертонічна хвороба", true));
        clinic.MedicalRecords.Add(new Diagnosis(1, 1, DateTime.Today.AddDays(-5), "J06.9", "Гострий ринофарингіт", false));
        clinic.MedicalRecords.Add(new LabResult(1, 1, DateTime.Today, "Гемоглобін", 145, "г/л", "120–160", true));
        clinic.MedicalRecords.Add(new LabResult(1, 1, DateTime.Today, "Холестерин", 6.2, "ммоль/л", "< 5.2", false));
        clinic.MedicalRecords.Add(new Prescription(1, 1, DateTime.Today.AddDays(-5), "Лізиноприл", "10 мг", 30, "1 раз на добу вранці"));

        clinic.MedicalRecords.Add(new Prescription(2, 2, DateTime.Today.AddDays(-40), "Амоксицилін", "500 мг", 10, "3 рази на добу"));
        clinic.MedicalRecords.Add(new Diagnosis(2, 2, DateTime.Today, "G43", "Мігрень", true));
        clinic.MedicalRecords.Add(new LabResult(3, 1, DateTime.Today, "Цукор", 4.5, "ммоль/л", "3.3–5.5", true));

        while (true)
        {
            Console.WriteLine("\n=== Головне меню ===");
            Console.WriteLine("1. Пацієнти");
            Console.WriteLine("2. Лікарі");
            Console.WriteLine("3. Прийоми");
            Console.WriteLine("4. Медична картка");
            Console.WriteLine("5. Звіт");
            Console.WriteLine("0. Вихід");
            Console.Write("Оберіть: ");

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    clinic.Patients.DisplayAll();
                    break;
                case "2":
                    clinic.Doctors.DisplayAll();
                    break;
                case "3":
                    clinic.Appointments.DisplayList(clinic.Appointments.GetUpcoming());
                    break;
                case "4":
                    MedicalRecordsMenu(clinic);
                    break;
                case "5":
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

    static void MedicalRecordsMenu(Clinic clinic)
    {
        while (true)
        {
            Console.WriteLine("\n── Медична картка ────────────");
            Console.WriteLine("1. Картка пацієнта");
            Console.WriteLine("2. Усі записи пацієнта");
            Console.WriteLine("3. Додати діагноз");
            Console.WriteLine("4. Додати аналіз");
            Console.WriteLine("5. Додати рецепт");
            Console.WriteLine("6. Записи лікаря");
            Console.WriteLine("0. Назад");
            Console.Write("Оберіть: ");

            string choice = Console.ReadLine() ?? "";

            if (choice == "0") return;

            try
            {
                if (choice == "1" || choice == "2")
                {
                    Console.Write("ID пацієнта: ");
                    if (!int.TryParse(Console.ReadLine(), out int pid))
                    {
                        Console.WriteLine("Помилка: некоректне число.");
                        continue;
                    }
                    if (clinic.Patients.FindById(pid) == null)
                    {
                        Console.WriteLine("Помилка: пацієнта не знайдено.");
                        continue;
                    }

                    if (choice == "1") clinic.MedicalRecords.DisplayPatientSummary(pid);
                    else clinic.MedicalRecords.DisplayList(clinic.MedicalRecords.GetByPatient(pid));
                }
                else if (choice == "6")
                {
                    Console.Write("ID лікаря: ");
                    if (!int.TryParse(Console.ReadLine(), out int did))
                    {
                        Console.WriteLine("Помилка: некоректне число.");
                        continue;
                    }
                    if (clinic.Doctors.FindById(did) == null)
                    {
                        Console.WriteLine("Помилка: лікаря не знайдено.");
                        continue;
                    }
                    clinic.MedicalRecords.DisplayList(clinic.MedicalRecords.GetByDoctor(did));
                }
                else if (choice == "3" || choice == "4" || choice == "5")
                {
                    Console.Write("ID пацієнта: ");
                    if (!int.TryParse(Console.ReadLine(), out int pid)) { Console.WriteLine("Некоректне число."); continue; }
                    if (clinic.Patients.FindById(pid) == null) { Console.WriteLine("Пацієнта не знайдено."); continue; }

                    Console.Write("ID лікаря: ");
                    if (!int.TryParse(Console.ReadLine(), out int did)) { Console.WriteLine("Некоректне число."); continue; }
                    if (clinic.Doctors.FindById(did) == null) { Console.WriteLine("Лікаря не знайдено."); continue; }

                    if (choice == "3")
                    {
                        Console.Write("Код діагнозу (напр. J06.9): ");
                        string code = Console.ReadLine() ?? "";
                        Console.Write("Опис: ");
                        string desc = Console.ReadLine() ?? "";
                        Console.Write("Хронічне? (1=так, 0=ні): ");
                        bool isChronic = (Console.ReadLine() == "1");

                        Diagnosis d = new Diagnosis(pid, did, DateTime.Today, code, desc, isChronic);
                        clinic.MedicalRecords.Add(d);
                    }
                    else if (choice == "4")
                    {
                        Console.Write("Назва аналізу: ");
                        string name = Console.ReadLine() ?? "";
                        Console.Write("Значення (число): ");
                        if (!double.TryParse(Console.ReadLine(), NumberStyles.Float, CultureInfo.InvariantCulture, out double val))
                        {
                            Console.WriteLine("Помилка: некоректне число.");
                            continue;
                        }
                        Console.Write("Одиниці виміру: ");
                        string unit = Console.ReadLine() ?? "";
                        Console.Write("Норма (напр. 4.0–9.0): ");
                        string refRange = Console.ReadLine() ?? "";
                        Console.Write("В нормі? (1=так, 0=ні): ");
                        bool isNormal = (Console.ReadLine() == "1");

                        LabResult lr = new LabResult(pid, did, DateTime.Today, name, val, unit, refRange, isNormal);
                        clinic.MedicalRecords.Add(lr);
                    }
                    else if (choice == "5")
                    {
                        Console.Write("Препарат: ");
                        string med = Console.ReadLine() ?? "";
                        Console.Write("Дозування (напр. 10 мг): ");
                        string dos = Console.ReadLine() ?? "";
                        Console.Write("Кількість днів (ціле): ");
                        if (!int.TryParse(Console.ReadLine(), out int days))
                        {
                            Console.WriteLine("Помилка: некоректне число.");
                            continue;
                        }
                        Console.Write("Інструкція (можна пропустити): ");
                        string instr = Console.ReadLine() ?? "";

                        Prescription p = new Prescription(pid, did, DateTime.Today, med, dos, days, instr);
                        clinic.MedicalRecords.Add(p);
                    }
                }
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine($"Помилка (діапазон): {e.Message}");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"Помилка (дані): {e.Message}");
            }
        }
    }
}