using System;
using System.Collections.Generic;

namespace EmployeeManagement;
public class EmployeeBAL : IEmployeeBAL
{
    private readonly IEmployeeDAL _employeeDal;
    private readonly IRolesBAL _rolesBAL;

    public EmployeeBAL(IEmployeeDAL employeeDAL, IRolesBAL rolesBAL)
    {
        _employeeDal = employeeDAL;
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
        var departments = _rolesBAL.GetAllDepartments();
        var department = departments.FirstOrDefault(d => d.Id == employee.DepartmentId);
        if (department == null)
        {
            return false; 
        }
        var roles = _rolesBAL.GetAllRoles().Where(r => r.DepartmentId == employee.DepartmentId);
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