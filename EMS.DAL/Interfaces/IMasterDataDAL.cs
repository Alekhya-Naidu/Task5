using System.Collections.Generic;

namespace EmployeeManagement;

public interface IMasterDataDal
{
    List<T> LoadData<T>(string filePath);
    Location GetLocationFromName(string locationInput);
    Department GetDepartmentFromName(string departmentInput);
    Manager GetManagerFromName(string managerInput);
    Project GetProjectFromName(string projectInput);
    Location GetLocationById(int locationId);
    Department GetDepartmentById(int departmentId);
    Manager GetManagerById(int managerId);
    Project GetProjectById(int projectId);
    int GetLocationId(Location location);
    int GetDepartmentId(Department department);
    int GetManagerId(Manager manager);
    int GetProjectId(Project project);
}