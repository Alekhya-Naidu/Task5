using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using EMS.BAL.Interfaces;
using EMS.DAL.Interfaces;
using EMS.DAL.DBO;

namespace EMS.BAL.BAL;
public class EmployeeBAL : IEmployeeBAL
{
    private readonly IConfiguration _configuration;
    private readonly IEmployeeDAL _employeeDal;
     private readonly IMasterDataBal _masterDataBAL;
    private readonly IRolesBAL _rolesBAL;

    public EmployeeBAL(IConfiguration configuration, IEmployeeDAL employeeDAL,IMasterDataBal masterDataBal, IRolesBAL rolesBAL)
    {
        _configuration = configuration;
        _employeeDal = employeeDAL;
        _masterDataBAL = masterDataBal;
        _rolesBAL = rolesBAL;
    }

    public bool Add(Employee employee)
    {
        if (!(ValidateEmployeeInputData(employee)))
        {
            return false;
        }
        try
        {
            return _employeeDal.Insert(employee);
        }
        catch
        {
            return false;
        }
    }

    public bool Update(Employee updatedEmployee)
    {
        List<Employee> existingEmployees = _employeeDal.GetAll();
        Employee existingEmployee = existingEmployees.Find(emp => emp.EmpNo == updatedEmployee.EmpNo);
        if (!(ValidateEmployeeInputData(updatedEmployee)))
        {
            return false;
        }
        if (existingEmployee != null)
        {
            return _employeeDal.Update(updatedEmployee);
        }
        return true;
    }

    public bool Delete(IEnumerable<int> empNos)
    {
        try
        {
            foreach (var empNo in empNos)
            {
                if (!_employeeDal.Delete(empNo))
                {
                    return false;
                }
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    private bool ValidateEmployeeInputData(Employee employee)
    {
        if(employee == null || employee.EmpNo <= 0)
        {
            return false;
        }
        if (string.IsNullOrWhiteSpace(employee.FirstName) || string.IsNullOrWhiteSpace(employee.LastName))
        {
            return false; 
        }
        if(string.IsNullOrWhiteSpace(employee.Mail) || !(employee.Mail.Contains("@") && employee.Mail.Contains(".")))
        {
            return false;
        }
        if(employee.MobileNumber.Length > 1)
        {
            if(employee.MobileNumber.Length != 10)
            {
               return false;
            }
        }
        if(employee.LocationId != 0)
        {
            if (employee.LocationId < 1 || employee.LocationId > 3)
            {
                return false;
            }
        }
        if(_rolesBAL == null)
        {
            return false;
        }
        var departments = _masterDataBAL.GetAllDepartments<Department>(Path.Combine(_configuration?["BaseFilePath"], _configuration?["DepartmentDataFilePath"]));
        var department = departments.FirstOrDefault(d => d.Id == employee.DepartmentId);
        if (department == null)
        {
            return false; 
        }
        var roles = _rolesBAL.GetAllRoles<Role>(Path.Combine(_configuration?["BaseFilePath"],_configuration?["RoleDataFilePath"])).Where(r => r.DepartmentId == employee.DepartmentId);
        if (!roles.Any())
        {
            return false; 
        }

        if(employee.ProjectId != 0)
        {
            if(employee.ProjectId != 1 && employee.ProjectId != 2)
            {
                return false;
            }
        }
        return true;
    }
    
    public List<Employee> Filter(EmployeeFilter filters)
    {
        return _employeeDal.Filter(filters);
    }

    public List<Employee> GetAllEmployees()
    {
        return _employeeDal.GetAll();
    }
}