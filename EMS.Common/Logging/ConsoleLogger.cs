using System;

namespace EmployeeManagement;

public class ConsoleLogger : ILogger
{
    public void LogInfo(string message)
    {
        Console.WriteLine(message);
    }

    public void DisplayMsg(string message)
    {
        Console.WriteLine(message);
    }

    public void LogError(string message)
    {
        LogMsg(message, ConsoleColor.Red);
    }

    public void LogSuccess(string message)
    {
        LogMsg(message, ConsoleColor.Green);
    }

    private void LogMsg(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}
