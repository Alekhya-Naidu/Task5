using System;

namespace EmployeeManagement;
public class ConsoleLogger : ILogger
{
    public void LogInfo(string message)
    {
        Console.WriteLine(message);
    }

    public void LogError(string message)
    {
        SetConsoleColor(ConsoleColor.Red);
        Console.WriteLine(message);
        ResetConsoleColor();
    }

    public void LogSuccess(string message)
    {
        SetConsoleColor(ConsoleColor.Green);
        Console.WriteLine(message);
        ResetConsoleColor();
    }

    private void SetConsoleColor(ConsoleColor color)
    {
        Console.ForegroundColor = color;
    }

    private void ResetConsoleColor()
    {
        Console.ResetColor();
    }
}
