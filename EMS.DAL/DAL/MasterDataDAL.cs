using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

using System.Text.Json;

namespace EmployeeManagement;

public class MasterDataDAl : IMasterDataDal
{
    private readonly IConfiguration _configuration;
    private readonly IJsonHelper _jsonHelper;

    public MasterDataDAl(IConfiguration configuration, IJsonHelper jsonHelper)
    {
        _configuration = configuration;
        _jsonHelper = jsonHelper;
    }

    public List<T> LoadData<T>(string filePath)
    {
        try
        {
            string json = _jsonHelper.ReadFromFile(filePath);
            return _jsonHelper.Deserialize<List<T>>(json);
        }
        catch
        {
            return new List<T>();
        }
    }
    
    public Location GetLocationFromName(string locationInput)
    {
        List<Location> locations = LoadData<Location>(Path.Combine(_configuration?["BaseFilePath"],_configuration?["LocationDataFilePath"]));
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
        List<Department> departments = LoadData<Department>(Path.Combine(_configuration?["BaseFilePath"], _configuration?["DepartmentDataFilePath"]));
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
        List<Manager> managers = LoadData<Manager>(Path.Combine(_configuration?["BaseFilePath"],_configuration?["ManagerDataFilePath"]));
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
        List<Project> projects = LoadData<Project>(Path.Combine(_configuration?["BaseFilePath"],_configuration?["ProjectDataFilePath"]));
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
        List<Location> locations = LoadData<Location>(Path.Combine(_configuration?["BaseFilePath"],_configuration?["LocationDataFilePath"]));
        return locations.FirstOrDefault(location => location.Id == locationId);
    }

    public Department GetDepartmentById(int departmentId)
    {
        List<Department> departments = LoadData<Department>(Path.Combine(_configuration?["BaseFilePath"], _configuration?["DepartmentDataFilePath"]));
        return departments.FirstOrDefault(department => department.Id == departmentId);
    }

    public Manager GetManagerById(int managerId)
    {
        List<Manager> managers = LoadData<Manager>(Path.Combine(_configuration?["BaseFilePath"],_configuration?["ManagerDataFilePath"]));
        return managers.FirstOrDefault(manager => manager.Id == managerId);
    }

    public Project GetProjectById(int projectId)
    {
        List<Project> projects = LoadData<Project>(Path.Combine(_configuration?["BaseFilePath"],_configuration?["ProjectDataFilePath"]));
        return projects.FirstOrDefault(project => project.Id == projectId);
    }
    
    public int GetLocationId(Location location)
    {
        if (location != null)
        {
            if (location.Id != 0)
            {
                return location.Id;
            }
            else if (!string.IsNullOrEmpty(location.Name))
            {
                Location existingLocation = GetLocationFromName(location.Name);
                return existingLocation?.Id ?? 0;
            }
        }
        return 0;
    }

    public int GetDepartmentId(Department department)
    {
        if(department != null)
        {
            if(department.Id != 0)
            {
                return department.Id;
            }
            else if(!string.IsNullOrEmpty(department.Name))
            {
                Department existingDepartment =  GetDepartmentFromName(department.Name);
                return existingDepartment?.Id ?? 0;
            }
        }
        return 0;
    }

    public int GetManagerId(Manager manager)
    {
        if(manager != null)
        {
            if(manager.Id != 0)
            {
                return manager.Id;
            }
            else if(!string.IsNullOrEmpty(manager.Name))
            {
                Manager existingManager =  GetManagerFromName(manager.Name);
                return existingManager?.Id ?? 0;
            }
        }
        return 0;
    }

    public int GetProjectId(Project project)
    {
        if(project != null)
        {
            if(project.Id != 0)
            {
                return project.Id;
            }
            else if(!string.IsNullOrEmpty(project.Name))
            {
                Project existingProject =  GetProjectFromName(project.Name);
                return existingProject?.Id ?? 0;
            }
        }
        return 0;
    }
}