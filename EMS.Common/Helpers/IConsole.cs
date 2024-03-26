using System;

namespace EmployeeManagement;

public interface IConsole
{
    void PrintMsg(string msg);
    void PrintError(string message);
    void PrintSuccess(string message);
}