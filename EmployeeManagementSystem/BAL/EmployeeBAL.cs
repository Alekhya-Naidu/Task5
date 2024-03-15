using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace EmployeeManagement;

public class EmployeeBAL : IBAL
{
    public readonly IDAL _dal;
    public readonly ILogger _console;
    public EmployeeBAL(IDAL dal, ILogger console)
    {
        _dal = dal;
        _console = console;
    }

    public void Add(Employee employee)
    {
        if(!(ValidateEmployeeInputData(employee)))
        {
            _console.LogError("Invalid Input");
            return;
        }
        try
        {
            _dal.Insert(employee);
        }
        catch (Exception ex)
        {
            _console.LogError($"Failed to add employee: {ex.Message}");
        }
    }

    public void Update(Employee updatedEmployee)
    {
        List<Employee> existingEmployees = _dal.Get();
        Employee existingEmployee = existingEmployees.FirstOrDefault(emp => emp.EmpNo == updatedEmployee.EmpNo);
        if (!(ValidateEmployeeInputData(updatedEmployee)))
        {
            _console.LogError("Invalid Input");
            return;
        }
        if (existingEmployee != null)
        {
            existingEmployee.FirstName = updatedEmployee.FirstName;
            existingEmployee.LastName = updatedEmployee.LastName;
            existingEmployee.Dob = updatedEmployee.Dob;
            existingEmployee.Mail = updatedEmployee.Mail;
            existingEmployee.MobileNumber = updatedEmployee.MobileNumber;
            existingEmployee.JoiningDate = updatedEmployee.JoiningDate;
            existingEmployee.Location = updatedEmployee.Location;
            existingEmployee.Department = updatedEmployee.Department;
            existingEmployee.Role = updatedEmployee.Role;
            existingEmployee.Manager = updatedEmployee.Manager;
            existingEmployee.Project = updatedEmployee.Project;
             try
            {
                _dal.Update(existingEmployees);
            }
            catch (Exception ex)
            {
                _console.LogError($"Failed to update employee: {ex.Message}");
            }
        }
        else
        {
            _console.LogError($"Employee with EmpNo {updatedEmployee.EmpNo} not found.");
        }
    }

    public void Delete(IEnumerable<int> empNos)
    {
        foreach(var emp in empNos)
        { 
            _dal.Delete(emp);
        }
    }
    
    public bool ValidateEmployeeInputData(Employee employee)
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
        if(employee.MobileNumber.Length != 10)
        {
            return false;
        }
        if(!Enum.IsDefined(typeof(Location), employee.Location))
        {
            return false;
        }
        if(!Enum.IsDefined(typeof(Department), employee.Department))
        {
            return false;
        }
        if(!Enum.IsDefined(typeof(Role), employee.Role))
        {
            return false;
        }
        if (!Enum.IsDefined(typeof(Manager), employee.Manager))
        {
            return false;
        }
        if (!Enum.IsDefined(typeof(Project), employee.Project))
        {
            return false;
        }
        return true;
    }

    public List<Employee> Filter(List<string> filters)
    {
        List<Employee> employees = _dal.Get();
        List<Employee> filteredEmps = new List<Employee>();

        foreach (var emp in employees)
        {
            bool isFiltered = false;
            foreach (var filter in filters)
            {
                if (emp.FirstName.ToLower().StartsWith(filter.ToLower()) ||
                    emp.Location.ToString().ToLower() == filter.ToLower() ||
                    emp.Department.ToString().ToLower() == filter.ToLower() ||
                    emp.EmpNo.ToString().ToLower() == filter.ToLower())
                    {
                        isFiltered = true;
                        break;
                    }
            }
            if (isFiltered)
            {
                filteredEmps.Add(emp);
            }
        }
        return filteredEmps;
    }

    public List<Employee> GetAllEmployees()
    {
        return _dal.Get();
    }
}