using System.Collections.Generic;
namespace EmployeeManagement;
public interface ILogger
{
    void LogMsg(string message);
    void LogError(string message);
    void LogSuccessful(string message);
    void LogEmployeeData(EmployeeManagementSystem.Employee employee);
    void LogFilteredEmployeeData(List<EmployeeManagementSystem.Employee> filteremp);
}