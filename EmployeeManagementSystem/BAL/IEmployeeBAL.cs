using System.Collections.Generic;

namespace EmployeeManagement;

public interface IEmployeeBAL
{
    Location GetLocationFromInput(string locationInput);
    Department GetDepartmentFromInput(string departmentName);
    Role GetRoleFromInput(string roleName);
    Manager GetManagerFromInput(string managerName);
    Project GetProjectFromInput(string projectName);
    Location GetLocationById(int locationId);
    Department GetDepartmentById(int departmentId);
    Role GetRoleById(int roleId);
    Manager GetManagerById(int managerId);
    Project GetProjectById(int projectId);
    bool Add(Employee employee) ;
    bool Update(Employee updatedEmployee);
    bool Delete(IEnumerable<int> empNos);
    List<Employee> Filter(EmployeeFilter filters);
    List<Employee> GetAllEmployees();
}