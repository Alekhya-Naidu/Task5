using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EmployeeManagement;

public class EmployeeDAL : IDAL
{
    private readonly ILogger _console;
    private readonly string _filePath;
    public EmployeeDAL(ILogger console)
    {
        _console = console;
        _filePath = GetFilePath();
    }

    public void Insert(Employee employee)
    {
        List<Employee> existingEmployees = Get();
        existingEmployees.Add(employee);
        Update(existingEmployees);    
    }
    
    public void Update(List<Employee> existingEmployees)
    {
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        string updatedJson = JsonSerializer.Serialize(existingEmployees, options);
        File.WriteAllText(_filePath, updatedJson);
    }

    public void Delete(int empNo)
    {
        List<Employee> employees = Get();
        Employee employeeTobeRemoved = employees.FirstOrDefault(emp => emp.EmpNo == empNo);
        if(employeeTobeRemoved != null)
        {
            employees.Remove(employeeTobeRemoved);
            Update(employees);
        }
        else
        {
            _console.LogError("Employee with EmpNo "+ empNo+" is not found");
        }
    }

    public List<Employee> Get()
    {
        List<Employee> existingEmployees = new List<Employee>();
        if (File.Exists(_filePath))
        {
            string json = File.ReadAllText(_filePath);
            existingEmployees = JsonSerializer.Deserialize<List<Employee>>(json) ?? new List<Employee>();
        }
        return existingEmployees;
    }

    public string GetFilePath()
    {
        try
        {
            string appSettingsPath = "appSettings.json";
            if (File.Exists(appSettingsPath))
            {
                string appSettingsJson = File.ReadAllText(appSettingsPath);
                var settings = JsonSerializer.Deserialize<Dictionary<string, string>>(appSettingsJson);
                if (settings.ContainsKey("EmployeeDataFilePath"))
                {
                    return settings["EmployeeDataFilePath"];
                }
            }
            _console.LogError("Employee data file path not found in app settings.");
        }
        catch (Exception ex)
        {
            _console.LogError($"Error reading app settings: {ex.Message}");
        }
        return null;
    }
}