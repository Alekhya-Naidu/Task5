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
    
    public EmployeeDAL(ILogger logger, string filePath)
    {
        _logger = logger;       
        _filePath = filePath;     
    }

    public bool Insert(Employee employee)
    {
        try
        {
            List<Employee> existingEmployees = GetAll();
            existingEmployees.Add(employee);
            SaveToJson(existingEmployees); 
            return true;
        }
        catch (Exception ex)
        {
            return false;
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
            existingEmployee.Location = employeeToBeUpdated.Location;
            existingEmployee.Department = employeeToBeUpdated.Department;
            existingEmployee.Role = employeeToBeUpdated.Role;
            existingEmployee.Manager = employeeToBeUpdated.Manager;
            existingEmployee.Project = employeeToBeUpdated.Project;
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
        catch (Exception ex)
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
            Employee employeeTobeRemoved = employees.FirstOrDefault(emp => emp.EmpNo == empNo);
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
        catch(Exception ex)
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
            bool isFiltered = 
                (string.IsNullOrEmpty(filters.FirstName) || emp.FirstName.ToLower().StartsWith(filters.FirstName.ToLower())) &&
                (string.IsNullOrEmpty(filters.Location) || emp.Location.ToString().ToLower() == filters.Location.ToLower()) &&
                (string.IsNullOrEmpty(filters.Department) || emp.Department.ToString().ToLower() == filters.Department.ToLower()) &&
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

