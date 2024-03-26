using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using EMS.DAL.DBO;
using EMS.DAL.Interfaces;  
using EMS.Common.Helpers;

namespace EMS.DAL.DAL;

public class EmployeeDAL : IEmployeeDAL
{
    private readonly IConfiguration _configuration;
    private readonly IJsonHelper _jsonHelper;
    private readonly IMasterDataDal _masterDataDAL;
    private readonly IRolesDAL _rolesDal;
    
    public EmployeeDAL(IConfiguration configuration, IJsonHelper jsonHelper, IMasterDataDal masterDataDal, IRolesDAL rolesDal)
    {
        _configuration = configuration;
        _jsonHelper = jsonHelper;
        _masterDataDAL = masterDataDal;
        _rolesDal = rolesDal;
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
            List<Location> locations = _masterDataDAL.GetAllLocations<Location>(Path.Combine(_configuration?["BaseFilePath"],_configuration?["LocationDataFilePath"]));
            List<Department> departments = _masterDataDAL.GetAllDepartments<Department>(Path.Combine(_configuration?["BaseFilePath"], _configuration?["DepartmentDataFilePath"]));
            List<Role> roles = _rolesDal.GetAllRoles<Role>(Path.Combine(_configuration?["BaseFilePath"],_configuration?["RoleDataFilePath"]));
            List<Manager> managers = _masterDataDAL.GetAllManagers<Manager>(Path.Combine(_configuration?["BaseFilePath"],_configuration?["ManagerDataFilePath"]));
            List<Project> projects = _masterDataDAL.GetAllProjects<Project>(Path.Combine(_configuration?["BaseFilePath"],_configuration?["ProjectDataFilePath"]));

            Location location = locations.FirstOrDefault(l => l.Name.Equals(employee.Location.Name, StringComparison.OrdinalIgnoreCase));
            Department department = departments.FirstOrDefault(d => d.Name.Equals(employee.Department.Name, StringComparison.OrdinalIgnoreCase));
            Role role = roles.FirstOrDefault(r => r.Name.Equals(employee.Role.Name, StringComparison.OrdinalIgnoreCase));
            Manager manager = managers.FirstOrDefault(m => m.Name.Equals(employee.Manager.Name, StringComparison.OrdinalIgnoreCase));
            Project project = projects.FirstOrDefault(p => p.Name.Equals(employee.Project.Name, StringComparison.OrdinalIgnoreCase));
            
            employee.LocationId = location.Id;
            employee.DepartmentId = department.Id;
            employee.RoleId = role.Id;
            employee.ManagerId = manager.Id;
            employee.ProjectId = project.Id;
        }
        catch{}
    }

    public bool Update(Employee employeeToBeUpdated)
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

    private bool SaveToJson(List<Employee> existingEmployees)
    {
        string updatedJson = _jsonHelper.Serialize(existingEmployees);
        _jsonHelper.WriteToFile(Path.Combine(_configuration?["BaseFilePath"],_configuration?["EmployeeDataFilePath"]), updatedJson);
        return true;
    }

    public bool Delete(int empNo)
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

    public List<Employee> Filter(EmployeeFilter filters)
    {
        List<Employee> employees = GetAll();
        List<Employee> filteredEmps = new List<Employee>();
        foreach (var emp in employees)
        {
            bool isFiltered = true;
            if (!string.IsNullOrEmpty(filters.FirstName) && !emp.FirstName.ToLower().StartsWith(filters.FirstName.ToLower()))
            {
                isFiltered = false;
            }
            if (filters.LocationId.HasValue && emp.LocationId != filters.LocationId)
            {
                isFiltered = false;
            }
            if (filters.DepartmentId.HasValue && emp.DepartmentId != filters.DepartmentId)
            {
                isFiltered = false;
            }
            if (filters.EmpNo.HasValue && emp.EmpNo != filters.EmpNo)
            {
                isFiltered = false;
            }
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
        if (File.Exists(Path.Combine(_configuration["BaseFilePath"],_configuration["EmployeeDataFilePath"])))
        {
            string json = _jsonHelper.ReadFromFile(Path.Combine(_configuration?["BaseFilePath"],_configuration?["EmployeeDataFilePath"]));
            return _jsonHelper.Deserialize<List<Employee>>(json);
        }
        return existingEmployees;
    }
}

