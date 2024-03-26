using System.Collections.Generic;

namespace EmployeeManagement;

public interface IEmployeeDAL
{
    bool Insert(Employee employee);
    bool Update(Employee employeeToBeUpdated);
    bool Delete(int empNo);
    List<Employee> Filter(EmployeeFilter filters);
    List<Employee> GetAll();
}