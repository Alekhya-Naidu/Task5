using System;
using System.Collections.Generic;

namespace EmployeeManagement;
public class ConsoleLogger : ILogger
{
    public void LogMsg(string message)
    {
        Console.WriteLine(message);
    }
    public void LogError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}