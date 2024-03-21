using System;
using System.Collections.Generic;

namespace EmployeeManagement;
    public class EmployeeBAL : IEmployeeBAL
    {
        private readonly IEmployeeDAL _employeeDal;
        private readonly IRolesBAL _rolesBAL; // Corrected variable name
        private readonly ILogger _logger;

        public EmployeeBAL(IEmployeeDAL employeeDAL, IRolesBAL rolesBAL, ILogger logger)
        {
            _employeeDal = employeeDAL;
            _rolesBAL = rolesBAL; // Corrected assignment
            _logger = logger;
        }

    public Location GetLocationFromInput(string locationInput)
    {
        return _employeeDal.GetLocationFromInput(locationInput);
    }

    public Department GetDepartmentFromInput(string departmentName)
    {
        return _employeeDal.GetDepartmentFromInput(departmentName);
    }

    public Role GetRoleFromInput(string roleName)
    {
        return _employeeDal.GetRoleFromInput(roleName);
    }

    public Manager GetManagerFromInput(string managerName)
    {
        return _employeeDal.GetManagerFromInput(managerName);
    }

    public Project GetProjectFromInput(string projectName)
    {
        return _employeeDal.GetProjectFromInput(projectName);
    }
    
    public Location GetLocationById(int locationId)
    {
        return _employeeDal.GetLocationById(locationId);
    }

    public Department GetDepartmentById(int departmentId)
    {
        return _employeeDal.GetDepartmentById(departmentId);
    }

    public Role GetRoleById(int roleId)
    {
        return _employeeDal.GetRoleById(roleId);
    }

    public Manager GetManagerById(int managerId)
    {
        return _employeeDal.GetManagerById(managerId);
    }

    public Project GetProjectById(int projectId)
    {
        return _employeeDal.GetProjectById(projectId);
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