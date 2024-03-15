using System.Collections.Generic;

namespace EmployeeManagement;

public interface ILogger
{
    void LogMsg(string message);
    void LogError(string message);
}