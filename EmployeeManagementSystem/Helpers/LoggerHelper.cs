using System;
using System.Collections.Generic;

namespace EmployeeManagement;
public class LoggerHandler : ILogger
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
    public void LogSuccessful(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(message);
        Console.ResetColor();
    }
    public void LogEmployeeData(EmployeeManagementSystem.Employee employee)
    {
        Console.WriteLine(employee.ToString());
    }
    public void LogFilteredEmployeeData(List<EmployeeManagementSystem.Employee> filterEmps)
    {
        foreach (var emp in filterEmps)
        {
            Console.WriteLine(emp.ToString());
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}