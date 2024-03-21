using System.Collections.Generic;

namespace EmployeeManagement;

public interface IEmployeeDAL
{
    Location GetLocationFromInput(string locationInput);
    Department GetDepartmentFromInput(string departmentInput);
    Role GetRoleFromInput(string roleInput);
    Manager GetManagerFromInput(string managerInput);
    Project GetProjectFromInput(string projectInput);
    Location GetLocationById(int locationId);
    Department GetDepartmentById(int departmentId);
    Role GetRoleById(int roleId);
    Manager GetManagerById(int managerId);
    Project GetProjectById(int projectId);
    bool Insert(Employee employee);
    bool Update(Employee employeeToBeUpdated);
    bool Delete(int empNo);
    int GetIdFromName<T>(List<T> dataList, string name) where T : IEntity;
    List<Employee> Filter(EmployeeFilter filters);
    List<Employee> GetAll();
}