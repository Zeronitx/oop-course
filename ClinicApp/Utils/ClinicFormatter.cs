using System;
using ClinicApp.Enums;

namespace ClinicApp.Utils;

public static class ClinicFormatter
{
    public static string FormatBloodType(BloodType bt)
    {
        return bt switch
        {
            BloodType.APositive => "A+",
            BloodType.ANegative => "A-",
            BloodType.BPositive => "B+",
            BloodType.BNegative => "B-",
            BloodType.ABPositive => "AB+",
            BloodType.ABNegative => "AB-",
            BloodType.OPositive => "O+",
            BloodType.ONegative => "O-",
            _ => "Unknown"
        };
    }

    public static string FormatSpeciality(Speciality s)
    {
        return s switch
        {
            Speciality.Cardiology => "Cardiology",
            Speciality.Neurology => "Neurology",
            Speciality.Pediatrics => "Pediatrics",
            Speciality.Surgery => "Surgery",
            Speciality.Orthopedics => "Orthopedics",
            Speciality.Dermatology => "Dermatology",
            Speciality.Emergency => "Emergency",
            _ => "General"
        };
    }

    public static string FormatAge(int age)
    {
        int lastTwoDigits = age % 100;
        int lastDigit = age % 10;

        if (lastTwoDigits >= 11 && lastTwoDigits <= 19)
            return $"{age} років";

        if (lastDigit == 1)
            return $"{age} рік";

        if (lastDigit >= 2 && lastDigit <= 4)
            return $"{age} роки";

        return $"{age} років";
    }

    public static string FormatPhone(string phone)
    {
        if (phone.Length == 10)
        {
            bool allDigits = true;
            foreach (char c in phone)
            {
                if (!char.IsDigit(c))
                {
                    allDigits = false;
                    break;
                }
            }

            if (allDigits)
            {
                return $"({phone.Substring(0, 3)}) {phone.Substring(3, 3)}-{phone.Substring(6, 4)}";
            }
        }
        return phone;
    }
}