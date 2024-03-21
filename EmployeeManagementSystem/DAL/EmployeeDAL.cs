using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace EmployeeManagement;

public class EmployeeDAL : IEmployeeDAL
{
    private readonly ILogger _logger;
    private readonly string _filePath;
    private readonly string _locationFilePath;
    private readonly string _departmentFilePath;
    private readonly string _roleFilePath;
    private readonly string _managerFilePath;
    private readonly string _projectFilePath;
    
    public EmployeeDAL(ILogger logger, string filePath, string locationFilePath, string departmentFilePath, string roleFilePath, string managerFilePath, string projectFilePath)
    {
        _logger = logger;
        _filePath = filePath;
        _locationFilePath = locationFilePath;
        _departmentFilePath = departmentFilePath;
        _roleFilePath = roleFilePath;
        _managerFilePath = managerFilePath;
        _projectFilePath = projectFilePath;
    }

    public Location GetLocationFromInput(string locationInput)
    {
        try
        {
            List<Location> locations = LoadData<Location>(_locationFilePath);
            foreach (var location in locations)
            {
                if (string.Equals(location.Name.ToLower(), locationInput, StringComparison.OrdinalIgnoreCase))
                {
                    return location;
                }
            }
            return null;
        }
        catch
        {
            return null;
        }
    }
    
    public Department GetDepartmentFromInput(string departmentInput)
    {
         try
        {
            List<Department> departments = LoadData<Department>(_departmentFilePath);
            foreach (var department in departments)
            {
                if (string.Equals(department.Name.ToLower(), departmentInput, StringComparison.OrdinalIgnoreCase))
                {
                    return department;
                }
            }
            return null;
        }
        catch
        {
            return null;
        }
    }
    
    public Role GetRoleFromInput(string roleInput)
    {
        try
        {
            List<Role> roles = LoadData<Role>(_roleFilePath);
            foreach (var role in roles)
            {
                if (string.Equals(role.Name.ToLower(), roleInput, StringComparison.OrdinalIgnoreCase))
                {
                    return role;
                }
            }
            return null;
        }
        catch
        {
            return null;
        }
    }
    
    public Manager GetManagerFromInput(string managerInput)
    {
        try
        {
            List<Manager> managers = LoadData<Manager>(_managerFilePath);
            foreach (var manager in managers)
            {
                if (string.Equals(manager.Name.ToLower(), managerInput, StringComparison.OrdinalIgnoreCase))
                {
                    return manager;
                }
            }
            return null;
        }    
        catch
        {
            return null;
        }
    }
    
    public Project GetProjectFromInput(string projectInput)
    {
        try
        {
            List<Project> projects = LoadData<Project>(_projectFilePath);
            foreach (var project in projects)
            {
                if (string.Equals(project.Name.ToLower(), projectInput, StringComparison.OrdinalIgnoreCase))
                {
                    return project;
                }
            }
            return null;
        }
        catch
        {
            return null;
        }
    }
    
    public Location GetLocationById(int locationId)
    {
        try
        {
            List<Location> locations = LoadData<Location>(_locationFilePath);
            return locations.FirstOrDefault(location => location.Id == locationId);
        }
        catch
        {
            return null;
        }
    }

    public Department GetDepartmentById(int departmentId)
    {
        try
        {
            List<Department> departments = LoadData<Department>(_departmentFilePath);
            return departments.FirstOrDefault(department => department.Id == departmentId);
        }
        catch
        {
            return null;
        }
    }

    public Role GetRoleById(int roleId)
    {
        try
        {
            List<Role> roles = LoadData<Role>(_roleFilePath);
            return roles.FirstOrDefault(role => role.Id == roleId);
        }
        catch
        {
            return null;
        }
    }

    public Manager GetManagerById(int managerId)
    {
        try
        {
            List<Manager> managers = LoadData<Manager>(_managerFilePath);
            return managers.FirstOrDefault(manager => manager.Id == managerId);
        }
        catch
        {
            return null;
        }
    }

    public Project GetProjectById(int projectId)
    {
        try
        {
            List<Project> projects = LoadData<Project>(_projectFilePath);
            return projects.FirstOrDefault(project => project.Id == projectId);
        }
        catch
        {
            return null;
        }
    }
    
    public bool Insert(Employee employee)
    {
        try
        {
            List<Employee> existingEmployees = GetAll();
            AssignIdsFromJson(employee);
            existingEmployees.Add(employee);
            SaveToJson(existingEmployees);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private void AssignIdsFromJson(Employee employee)
    {
        try
        {
            List<Location> locations = LoadData<Location>(_locationFilePath);
            List<Department> departments = LoadData<Department>(_departmentFilePath);
            List<Role> roles = LoadData<Role>(_roleFilePath);
            List<Manager> managers = LoadData<Manager>(_managerFilePath);
            List<Project> projects = LoadData<Project>(_projectFilePath);
            
            employee.LocationId = GetIdFromName(locations, employee.Location.Name);
            employee.DepartmentId = GetIdFromName(departments, employee.Department.Name);
            employee.RoleId = GetIdFromName(roles, employee.Role.Name);
            employee.ManagerId = GetIdFromName(managers, employee.Manager.Name);
            employee.ProjectId = GetIdFromName(projects, employee.Project.Name);
        }
        catch{}
    }

    public int GetIdFromName<T>(List<T> dataList, string name) where T : IEntity
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return 0;
        }
        foreach (var data in dataList)
        {
            var dataName = data?.Name?.Trim();
            if (dataName != null && string.Equals(dataName, name.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                return data.Id;
            }
        }
        return 0;
    }

    private List<T> LoadData<T>(string filePath)
    {
        try
        {
            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<T>>(json);
        }
        catch
        {
            return new List<T>();
        }
    }

    public bool Update(Employee employeeToBeUpdated)
    {
        try
        {
            List<Employee> existingEmployees = GetAll();
            Employee existingEmployee = existingEmployees.FirstOrDefault(emp => emp.EmpNo == employeeToBeUpdated.EmpNo);
            existingEmployee.FirstName = employeeToBeUpdated.FirstName;
            existingEmployee.LastName = employeeToBeUpdated.LastName;
            existingEmployee.Dob = employeeToBeUpdated.Dob;
            existingEmployee.Mail = employeeToBeUpdated.Mail;
            existingEmployee.MobileNumber = employeeToBeUpdated.MobileNumber;
            existingEmployee.JoiningDate = employeeToBeUpdated.JoiningDate;
            existingEmployee.LocationId = employeeToBeUpdated.LocationId;
            existingEmployee.DepartmentId = employeeToBeUpdated.DepartmentId;
            existingEmployee.RoleId = employeeToBeUpdated.RoleId;
            existingEmployee.ManagerId = employeeToBeUpdated.ManagerId;
            existingEmployee.ProjectId = employeeToBeUpdated.ProjectId;
            SaveToJson(existingEmployees);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private bool SaveToJson(List<Employee> existingEmployees)
    {
        try
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string updatedJson = JsonSerializer.Serialize(existingEmployees, options);
            WriteToFile(updatedJson);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private void WriteToFile(string json)
    {
        File.WriteAllText(_filePath, json);
    }

    public bool Delete(int empNo)
    {
        try
        {
            List<Employee> employees = GetAll();
            Employee? employeeTobeRemoved = employees.FirstOrDefault(emp => emp.EmpNo == empNo);
            if(employeeTobeRemoved != null)
            {
                employees.Remove(employeeTobeRemoved);
                SaveToJson(employees);
                return true;
            }
            else
            {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }

    public List<Employee> Filter(EmployeeFilter filters)
    {
        List<Employee> employees = GetAll();
        List<Employee> filteredEmps = new List<Employee>();

        foreach (var emp in employees)
        {
            var location = GetLocationById(emp.LocationId);
            var department = GetDepartmentById(emp.DepartmentId);

            string locationName = location?.Name ?? string.Empty;
            string departmentName = department?.Name ?? string.Empty;

            bool isFiltered =
                (string.IsNullOrEmpty(filters.FirstName) || emp.FirstName.ToLower().StartsWith(filters.FirstName.ToLower())) &&
                (string.IsNullOrEmpty(filters.LocationName) || string.Equals(locationName, filters.LocationName, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(filters.DepartmentName) || string.Equals(departmentName, filters.DepartmentName, StringComparison.OrdinalIgnoreCase)) &&
                (!filters.EmpNo.HasValue || emp.EmpNo == filters.EmpNo);

            if (isFiltered)
            {
                filteredEmps.Add(emp);
            }
        }
        return filteredEmps;
    }

    public List<Employee> GetAll()
    {
        List<Employee> existingEmployees = new List<Employee>();
        if (File.Exists(_filePath))
        {
            string json = File.ReadAllText(_filePath);
            existingEmployees = JsonSerializer.Deserialize<List<Employee>>(json) ?? new List<Employee>();
        }
        return existingEmployees;
    }
}

