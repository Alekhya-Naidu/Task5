using System.Collections.Generic;

namespace EmployeeManagement;

public interface IDAL
{
    void Insert(Employee employee);
    void Update(List<Employee> existingEmployees);
    void Delete(int empNo);
    List<Employee> Get();
    string GetFilePath();
}