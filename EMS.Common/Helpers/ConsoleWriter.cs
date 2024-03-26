using System;

namespace EmployeeManagement;

public class ConsoleWriter : IConsole
{
    public void PrintMsg(string msg)
    {
        Console.WriteLine(msg);
    }
    public void PrintError(string message)
    {
        PrintMsgWithColor(message, ConsoleColor.Red);
    }

    public void PrintSuccess(string message)
    {
        PrintMsgWithColor(message, ConsoleColor.Green);
    }

    private void PrintMsgWithColor(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}