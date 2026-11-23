using System;
using System.IO;
using System.Text;

namespace ClinicApp.Utils;

public class ClinicLogger
{
    private readonly string _logPath;

    public ClinicLogger(string logPath = "clinic.log")
    {
        _logPath = logPath;
    }

    private void Write(string level, string message)
    {
        string line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level,-5}] {message}{Environment.NewLine}";
        File.AppendAllText(_logPath, line, Encoding.UTF8);
    }

    public void LogInfo(string message)
    {
        Write("INFO", message);
    }

    public void LogWarning(string message)
    {
        Write("WARN", message);
    }

    public void LogError(string message)
    {
        Write("ERROR", message);
    }

    public string[] GetLastLines(int n)
    {
        if (!Exists() || n <= 0) return new string[0];

        string[] allLines = File.ReadAllLines(_logPath, Encoding.UTF8);
        int skip = allLines.Length > n ? allLines.Length - n : 0;
        int count = allLines.Length - skip;

        string[] result = new string[count];
        for (int i = 0; i < count; i++)
        {
            result[i] = allLines[skip + i];
        }
        return result;
    }

    public void Clear()
    {
        if (Exists())
        {
            File.Delete(_logPath);
        }
    }

    public bool Exists()
    {
        return File.Exists(_logPath);
    }
}