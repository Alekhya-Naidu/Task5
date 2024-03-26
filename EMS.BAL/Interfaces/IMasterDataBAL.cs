using System.Collections.Generic;
using EMS.DAL.DBO;

namespace EMS.BAL.Interfaces;

public interface IMasterDataBal
{
    List<Location> GetAllLocations<Location>(string locationfilePath);
    List<Department> GetAllDepartments<Department>(string departmentfilePath);
    List<Manager> GetAllManagers<Manager>(string managerfilePath);
    List<Project> GetAllProjects<Project>(string projectfilePath);
    Location GetLocationFromName(string locationInput);
    Department GetDepartmentFromName(string departmentName);
    Manager GetManagerFromName(string managerName);
    Project GetProjectFromName(string projectName);
    Location GetLocationById(int locationId);
    Department GetDepartmentById(int departmentId);
    Manager GetManagerById(int managerId);
    Project GetProjectById(int projectId);
}