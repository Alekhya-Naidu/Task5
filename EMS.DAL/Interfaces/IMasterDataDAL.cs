using System.Collections.Generic;
using EMS.DAL.DBO;

namespace EMS.DAL.Interfaces;

public interface IMasterDataDal
{
    List<Location> GetAllLocations<Location>(string locationfilePath);
    List<Department> GetAllDepartments<Department>(string departmentfilePath);
    List<Manager> GetAllManagers<Manager>(string managerfilePath);
    List<Project> GetAllProjects<Project>(string projectfilePath);
    Location GetLocationFromName(string locationInput);
    Department GetDepartmentFromName(string departmentInput);
    Manager GetManagerFromName(string managerInput);
    Project GetProjectFromName(string projectInput);
    Location GetLocationById(int locationId);
    Department GetDepartmentById(int departmentId);
    Manager GetManagerById(int managerId);
    Project GetProjectById(int projectId);
}