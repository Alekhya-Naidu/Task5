using System.Collections.Generic;

namespace EmployeeManagement;

public interface IBAL
{
    void Add(Employee employee) ;
    void Update(Employee updatedEmployee);
    void Delete(IEnumerable<int> empNos);
    bool ValidateEmployeeInputData(Employee employee);
    List<Employee> Filter(List<string> filters);
    List<Employee> GetAllEmployees();
}