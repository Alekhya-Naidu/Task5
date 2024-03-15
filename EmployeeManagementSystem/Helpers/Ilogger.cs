using System.Collections.Generic;

namespace EmployeeManagement;

public interface ILogger
{
    void LogInfo(string message);
    void LogError(string message);
    void LogSuccess(string message);
 
}