using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using EMS.DAL.DBO;
using EMS.DAL.Interfaces;  
using EMS.Common.Helpers;

namespace EMS.DAL.DAL;

public class MasterDataDAl : IMasterDataDal
{
    private readonly IConfiguration _configuration;
    private readonly IJsonHelper _jsonHelper;

    public MasterDataDAl(IConfiguration configuration, IJsonHelper jsonHelper)
    {
        _configuration = configuration;
        _jsonHelper = jsonHelper;
    }

    public List<Location> GetAllLocations<Location>(string locationfilePath)
    {
        try
        {
            string json = _jsonHelper.ReadFromFile(locationfilePath);
            return _jsonHelper.Deserialize<List<Location>>(json);
        }
        catch
        {
            return new List<Location>();
        }
    }
    

    public List<Department> GetAllDepartments<Department>(string departmentfilePath)
    {
        try
        {
            string json = _jsonHelper.ReadFromFile(departmentfilePath);
            return _jsonHelper.Deserialize<List<Department>>(json);
        }
        catch
        {
            return new List<Department>();
        }
    }
    

    public List<Manager> GetAllManagers<Manager>(string managerfilePath)
    {
        try
        {
            string json = _jsonHelper.ReadFromFile(managerfilePath);
            return _jsonHelper.Deserialize<List<Manager>>(json);
        }
        catch
        {
            return new List<Manager>();
        }
    }
    

    public List<Project> GetAllProjects<Project>(string projectfilePath)
    {
        try
        {
            string json = _jsonHelper.ReadFromFile(projectfilePath);
            return _jsonHelper.Deserialize<List<Project>>(json);
        }
        catch
        {
            return new List<Project>();
        }
    }
    

    public Location GetLocationFromName(string locationInput)
    {
        List<Location> locations = GetAllLocations<Location>(Path.Combine(_configuration?["BaseFilePath"],_configuration?["LocationDataFilePath"]));
        foreach (var location in locations)
        {
            if (string.Equals(location.Name.ToLower(), locationInput, StringComparison.OrdinalIgnoreCase))
            {
                return location;
            }
        }
        return null;
    }
    
    public Department GetDepartmentFromName(string departmentInput)
    {
        List<Department> departments = GetAllDepartments<Department>(Path.Combine(_configuration?["BaseFilePath"], _configuration?["DepartmentDataFilePath"]));
        foreach (var department in departments)
        {
            if (string.Equals(department.Name.ToLower(), departmentInput, StringComparison.OrdinalIgnoreCase))
            {
                return department;
            }
        }
        return null;
    }
    
    public Manager GetManagerFromName(string managerInput)
    {
        List<Manager> managers = GetAllManagers<Manager>(Path.Combine(_configuration?["BaseFilePath"],_configuration?["ManagerDataFilePath"]));
        foreach (var manager in managers)
        {
            if (string.Equals(manager.Name.ToLower(), managerInput, StringComparison.OrdinalIgnoreCase))
            {
                return manager;
            }
        }
        return null;
    }
    
    public Project GetProjectFromName(string projectInput)
    {
        List<Project> projects = GetAllProjects<Project>(Path.Combine(_configuration?["BaseFilePath"],_configuration?["ProjectDataFilePath"]));
        foreach (var project in projects)
        {
            if (string.Equals(project.Name.ToLower(), projectInput, StringComparison.OrdinalIgnoreCase))
            {
                return project;
            }
        }
        return null;
    }
    
    public Location GetLocationById(int locationId)
    {
        List<Location> locations = GetAllLocations<Location>(Path.Combine(_configuration?["BaseFilePath"],_configuration?["LocationDataFilePath"]));
        return locations.FirstOrDefault(location => location.Id == locationId);
    }

    public Department GetDepartmentById(int departmentId)
    {
        List<Department> departments = GetAllDepartments<Department>(Path.Combine(_configuration?["BaseFilePath"], _configuration?["DepartmentDataFilePath"]));
        return departments.FirstOrDefault(department => department.Id == departmentId);
    }

    public Manager GetManagerById(int managerId)
    {
        List<Manager> managers = GetAllManagers<Manager>(Path.Combine(_configuration?["BaseFilePath"],_configuration?["ManagerDataFilePath"]));
        return managers.FirstOrDefault(manager => manager.Id == managerId);
    }

    public Project GetProjectById(int projectId)
    {
        List<Project> projects = GetAllProjects<Project>(Path.Combine(_configuration?["BaseFilePath"],_configuration?["ProjectDataFilePath"]));
        return projects.FirstOrDefault(project => project.Id == projectId);
    }
}