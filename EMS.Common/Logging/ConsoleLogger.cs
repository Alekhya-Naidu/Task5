using System;

namespace EMS.Common.Logging;

public class ConsoleLogger : ILogger
{
    public void LogInfo(string message)
    {
        LogMessage(message, ConsoleColor.White);
    }

    public void LogError(string message)
    {
        LogMessage(message, ConsoleColor.Red);
    }

    public void LogSuccess(string message)
    {
        LogMessage(message, ConsoleColor.Green);
    }

    private void LogMessage(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}
