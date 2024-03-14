using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EmployeeManagement;
public class EmployeeDataAccess
{
    private readonly string fileName = "EmployeesData.json";
    private readonly ILogger ilogger;
    public EmployeeDataAccess(ILogger logger)
    {
        ilogger = logger;
    }
    public EmployeeManagementSystem.Employee FindEmployee(int empNo, ILogger ilogger)
    {       
        EmployeeManager empManager = new EmployeeManager(ilogger);
        List<EmployeeManagementSystem.Employee> existingEmployees = empManager.GetDataStored(fileName);
        if(existingEmployees.Count > 0)
        {
            EmployeeManagementSystem.Employee? SearchingEmp = existingEmployees.Find(emp => emp.EmpNo == empNo);

            if(SearchingEmp != null)
            {
                ilogger.LogMsg("Employee with empNo: "+empNo+ " is: \n");
                ilogger.LogEmployeeData(SearchingEmp);
                ilogger.LogSuccessful("Search Successful");
                return SearchingEmp;
            }
            else
            {
                ilogger.LogMsg("Employee with empNo: "+empNo +" not found.");
                return null;
            }
        }
        else
        {
            ilogger.LogMsg("No employees found.");
            return null;
        }
    }
    public void EditEmployee(int empNumber, ILogger ilogger)
    {
        EmployeeManager empManager = new EmployeeManager(ilogger);
        List<EmployeeManagementSystem.Employee> existingEmployees = empManager.GetDataStored(fileName);
        if(existingEmployees.Count > 0)
        {
            EmployeeManagementSystem.Employee? searchingEmp = existingEmployees.Find(emp => emp.EmpNo == empNumber);

            if(searchingEmp != null)
            {
                ilogger.LogSuccessful("Employee found, Enter new details");
                Program program = new Program(ilogger);
                EmployeeManagementSystem.Employee updatedEmployee = program.UpdateFromInput(searchingEmp);

                int index = existingEmployees.FindIndex(emp => emp.EmpNo == empNumber);
                if (index != -1)
                {
                    existingEmployees[index] = updatedEmployee;
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        WriteIndented = true
                    };
                    string updatedJson = JsonSerializer.Serialize(existingEmployees, options);
                    File.WriteAllText(fileName, updatedJson);
                    ilogger.LogSuccessful("Updated Successfully");
                }
                else
                {
                    ilogger.LogError("Error updating employee: Employee not found in the list");
                }
            }
            else
            {
                ilogger.LogError("No employee found to edit the data");
            }
        }
        else
        {
            ilogger.LogError("No employees data");
        }
    }
    public void FilterEmployeeDetails()
    {
        if(File.Exists(fileName))
        {
            EmployeeManager empManager = new EmployeeManager(ilogger);
            List<EmployeeManagementSystem.Employee> existingEmployees = empManager.GetDataStored(fileName);
            if(existingEmployees.Count > 0)
            {
                ilogger.LogMsg("1.Alphabet\n2.Location\n3.Department\n");
                ilogger.LogMsg("Enter the filtering criteria among the following");
                string? filterOption = Console.ReadLine();
                ilogger.LogMsg("");
                if(filterOption != null)
                {
                    filterOption = filterOption.ToLower();  
                    List<EmployeeManagementSystem.Employee> filteredEmployeeData = new List<EmployeeManagementSystem.Employee>();
                    switch(filterOption)
                    {
                        case "alphabet":
                            ilogger.LogMsg("Enter an Alphabet : ");
                            char alphabet = Console.ReadKey().KeyChar;
                            alphabet = char.ToLower(alphabet);
                            foreach(var emp in existingEmployees)
                            {
                                if(!string.IsNullOrEmpty(emp.FirstName) && (emp.FirstName.ToLower())[0] == alphabet)
                                {
                                    filteredEmployeeData.Add(emp);
                                }
                            }
                            break;
                        case "location":
                            ilogger.LogMsg("Enter a Location (Hyderabad, US, UK): ");
                            if (Enum.TryParse(Console.ReadLine(), true, out EmployeeManagementSystem.Location Location))
                            {
                                if(Location != null)
                                {
                                    foreach(var emp in existingEmployees)
                                    {
                                        if(emp.Location == Location)
                                        {
                                            filteredEmployeeData.Add(emp);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                ilogger.LogMsg("Enter location as Hyderabad or US or UK");
                            }
                            break;
                        case "department":
                            ilogger.LogMsg("Enter an Department (PE, IT) : ");
                            if (Enum.TryParse(Console.ReadLine(), true, out EmployeeManagementSystem.Department Department))
                            {
                                if(Department != null)
                                {
                                    foreach(var emp in existingEmployees)
                                    {
                                        if(emp.Department == Department)
                                        {
                                            filteredEmployeeData.Add(emp);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                ilogger.LogMsg("Enter PE or IT as department");
                            }
                            break;
                        default:
                            ilogger.LogError("Invalid Filter");
                            break;
                    }
                    if(filteredEmployeeData.Count > 0)
                    {
                        ilogger.LogSuccessful("\n\nFiltering Successful");
                        ilogger.LogFilteredEmployeeData(filteredEmployeeData);
                    }
                    else
                    {
                        ilogger.LogMsg("No data found for your filter");
                    }
                }
                else
                {
                    ilogger.LogMsg("No filter Selected");
                }
            }
            else
            {
                ilogger.LogError("No Employees Found");
            }
        }
        else
        {
            ilogger.LogError("File not found");
        }
    }
}