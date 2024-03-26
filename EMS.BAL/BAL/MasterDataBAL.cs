using System;
using System.Collections.Generic;
using EMS.BAL.Interfaces;
using EMS.DAL.Interfaces;
using EMS.DAL.DBO;

namespace EMS.BAL.BAL;

public class MasterDataBAL : IMasterDataBal
{
    private readonly IMasterDataDal _masterDataDAL;

    public MasterDataBAL(IMasterDataDal masterDataDal)
    {
        _masterDataDAL = masterDataDal;
    }
    
    public List<Location> GetAllLocations<Location>(string locationfilePath)
    {
        return _masterDataDAL.GetAllLocations<Location>(locationfilePath);
    }
    

    public List<Department> GetAllDepartments<Department>(string departmentfilePath)
    {
        return _masterDataDAL.GetAllDepartments<Department>(departmentfilePath);
    }
    

    public List<Manager> GetAllManagers<Manager>(string managerfilePath)
    {
        return _masterDataDAL.GetAllManagers<Manager>(managerfilePath);
    }
    

    public List<Project> GetAllProjects<Project>(string projectfilePath)
    {
        return _masterDataDAL.GetAllProjects<Project>(projectfilePath);
    }
    
    public Location GetLocationFromName(string locationInput)
    {
        return _masterDataDAL.GetLocationFromName(locationInput);
    }

    public Department GetDepartmentFromName(string departmentName)
    {
        return _masterDataDAL.GetDepartmentFromName(departmentName);
    }

    public Manager GetManagerFromName(string managerName)
    {
        return _masterDataDAL.GetManagerFromName(managerName);
    }

    public Project GetProjectFromName(string projectName)
    {
        return _masterDataDAL.GetProjectFromName(projectName);
    }
    
    public Location GetLocationById(int locationId)
    {
        return _masterDataDAL.GetLocationById(locationId);
    }

    public Department GetDepartmentById(int departmentId)
    {
        return _masterDataDAL.GetDepartmentById(departmentId);
    }

    public Manager GetManagerById(int managerId)
    {
        return _masterDataDAL.GetManagerById(managerId);
    }

    public Project GetProjectById(int projectId)
    {
        return _masterDataDAL.GetProjectById(projectId);
    }
}