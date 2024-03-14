using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace EmployeeManagement;
class EmployeeManager
{
    private readonly string fileName = "EmployeesData.json";
    private readonly ILogger ilogger;
    public EmployeeManager(ILogger logger)
    {
        ilogger = logger;
    }
    public void AddEmployee(ILogger logger)
    {
        Program program = new Program(logger);
        EmployeeManagementSystem.Employee employeeData =  program.EmployeeDataInput();
        if(employeeData != null && employeeData.EmpNo != 0)
        {
            ilogger.LogSuccessful("Employee added successfully.");
            SaveEmployeesToJson(employeeData);
        }
    }
    public void SaveEmployeesToJson(EmployeeManagementSystem.Employee employee)
    {
        string appSettingsPath = "appSettings.json";
        if (File.Exists(appSettingsPath))
        {
            string appSettingsJson = File.ReadAllText(appSettingsPath);
            JsonDocument settings = JsonDocument.Parse(appSettingsJson);
            JsonElement root = settings.RootElement;

            if (root.TryGetProperty("EmployeeDataFilePath", out JsonElement FilePath))
            {
                string filePath = FilePath.GetString();
                if (!string.IsNullOrEmpty(filePath))
                {
                    List<EmployeeManagementSystem.Employee> existingEmployees = GetDataStored(filePath);
                    existingEmployees.Add(employee);
                    Update(filePath, existingEmployees);
                }
            }
            else
            {
                ilogger.LogError("EmployeeDataFilePath is missing in appSettings.json");
            }
        }
        else
        {
            ilogger.LogError("App settings file not found.");
        }
    }
    public List<EmployeeManagementSystem.Employee> GetDataStored(string filePath)
    {
        List<EmployeeManagementSystem.Employee> existingEmployees = new List<EmployeeManagementSystem.Employee>();
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            existingEmployees = JsonSerializer.Deserialize<List<EmployeeManagementSystem.Employee>>(json) ?? new List<EmployeeManagementSystem.Employee>();
        }
        return existingEmployees;
    }
    public void Update(string filePath, List<EmployeeManagementSystem.Employee> existingEmployees)
    {
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        string updatedJson = JsonSerializer.Serialize(existingEmployees, options);
        File.WriteAllText(filePath, updatedJson);
    }
    public void DeleteEmployee(params int[] empNos)
    {
        List<EmployeeManagementSystem.Employee> existingEmployees = GetDataStored(fileName);
        if (existingEmployees.Count > 0)
        {
            bool deleted = false;
            ilogger.LogMsg("Do you want to delete (y/n)? :" );
            char confirm = Console.ReadKey().KeyChar;
            char confirmStatus = Char.ToLower(confirm);
            ilogger.LogMsg("");

            if(confirmStatus == 'y')
            {
                foreach (int empNo in empNos)
                {
                    EmployeeManagementSystem.Employee employeeToRemove = existingEmployees.Find(emp => emp.EmpNo == empNo);
                    if (employeeToRemove != null)
                    {
                        existingEmployees.Remove(employeeToRemove);
                        deleted = true;
                        ilogger.LogSuccessful("Employee with empNo "+empNo+" deleted successfully");
                    }
                    else
                    {
                        ilogger.LogError("Employee with empNo " + empNo +" not found.");
                    }
                }
                if (deleted)
                {
                    Update(fileName, existingEmployees);
                }
                else
                {
                    ilogger.LogError("No employees found.");
                }
            }
        } 
        else
        {
            ilogger.LogError("No employees to delete");
        }  
       
    }
}

    

