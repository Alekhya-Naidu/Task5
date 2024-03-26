using System.Collections.Generic;

namespace EmployeeManagement;

public interface IMasterDataBal
{
    Location GetLocationFromName(string locationInput);
    Department GetDepartmentFromName(string departmentName);
    Manager GetManagerFromName(string managerName);
    Project GetProjectFromName(string projectName);
    Location GetLocationById(int locationId);
    Department GetDepartmentById(int departmentId);
    Manager GetManagerById(int managerId);
    Project GetProjectById(int projectId);
}