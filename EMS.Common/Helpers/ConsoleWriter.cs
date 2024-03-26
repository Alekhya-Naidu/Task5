using System;

namespace EMS.Common.Helpers;

public class ConsoleWriter : IWriter
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