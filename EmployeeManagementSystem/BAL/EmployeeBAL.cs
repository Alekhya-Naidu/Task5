using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace EmployeeManagement;

public class EmployeeBAL : IEmployeeBAL
{
    public readonly IEmployeeDAL _employeeDal;
    public readonly ILogger _logger;
    
    public EmployeeBAL(IEmployeeDAL employeeDAL, ILogger logger)
    {
        _employeeDal = employeeDAL;
        _logger = logger;
    }

    public bool Add(Employee employee)
    {
        if(!(ValidateEmployeeInputData(employee)))
        {
            return false;
        }
        try
        {
            return _employeeDal.Insert(employee);
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public bool Update(Employee updatedEmployee)
    {
        List<Employee> existingEmployees = _employeeDal.GetAll();
        Employee existingEmployee = existingEmployees.FirstOrDefault(emp => emp.EmpNo == updatedEmployee.EmpNo);
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
        catch (Exception ex)
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
                return false;
        }
        if(employee.Location != 0)
        {
            if(!Enum.IsDefined(typeof(Location), employee.Location))
            {
                return false;
            }
            return true;
        }
        if(employee.Department != 0)
        {
            if(!Enum.IsDefined(typeof(Department), employee.Department))
            {
                return false;
            }
        }
        if(employee.Role != 0)
        {
            if(!Enum.IsDefined(typeof(Role), employee.Role))
            {
                return false;
            }
        }
        if(employee.Manager != 0)
        {
            if (!Enum.IsDefined(typeof(Manager), employee.Manager))
            {
                return false;
            }
        }
        if(employee.Project != 0)
        {
            if (!Enum.IsDefined(typeof(Project), employee.Project))
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