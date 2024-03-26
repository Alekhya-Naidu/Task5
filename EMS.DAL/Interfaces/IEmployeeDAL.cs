using System.Collections.Generic;
using EMS.DAL.DBO;

namespace EMS.DAL.Interfaces;

public interface IEmployeeDAL
{
    bool Insert(Employee employee);
    bool Update(Employee employeeToBeUpdated);
    bool Delete(int empNo);
    List<Employee> Filter(EmployeeFilter filters);
    List<Employee> GetAll();
}