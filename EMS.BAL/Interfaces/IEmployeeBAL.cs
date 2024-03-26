using System.Collections.Generic;
using EMS.DAL.DBO;

namespace EMS.BAL.Interfaces;

public interface IEmployeeBAL
{
    bool Add(Employee employee) ;
    bool Update(Employee updatedEmployee);
    bool Delete(IEnumerable<int> empNos);
    List<Employee> Filter(EmployeeFilter filters);
    List<Employee> GetAllEmployees();
}