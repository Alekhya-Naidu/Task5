using System;
using System.Collections.Generic;

namespace EmployeeManagement;

public class MasterDataBAL : IMasterDataBal
{
    private readonly IMasterDataDal _masterDataDAL;

    public MasterDataBAL(IMasterDataDal masterDataDal)
    {
        _masterDataDAL = masterDataDal;
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