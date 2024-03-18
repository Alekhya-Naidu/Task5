using System;
using CommandLine;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace EmployeeManagement;

public static class Program
{   
    private static readonly IEmployeeBAL _employeeBal;
    private static IConfiguration _configuration;
    private static readonly ILogger _logger;
    private static readonly string _filePath;
    
    static Program()
    {
        BuildConfiguration();
        _logger = new ConsoleLogger();
        _filePath = _configuration["EmployeeDataFilePath"];
        _employeeBal = new EmployeeBAL(new EmployeeDAL(_logger, _filePath), _logger);
    }
    
    public static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args)
        .WithParsed(options =>
        {
            if(options.Add)
            {
                if(_employeeBal.Add(GetEmployeeInputs()))
                {
                    _logger.LogSuccess("Succesfully Added ");
                }
                else
                {
                    _logger.LogError("Invalid");
                }
            }
            else if(options.Filter)
            {
                FilterAndDisplay();
            }
            else if(options.Edit != 0)
            {
                Update();
            }
            else if (options.Delete != null && options.Delete.Any())
            {
                if(_employeeBal.Delete(options.Delete))
                {
                    _logger.LogSuccess("Deleted Successfully");
                }
                else
                {
                    _logger.LogSuccess("Cannot find Employee");
                }
            }
            else if(options.Display)
            {
                List<Employee> employees = _employeeBal.GetAllEmployees();
                DisplayEmployee(employees);
            }
            else
            {
                _logger.LogError("Invalid Command");
            }
        });
    }

    public static void BuildConfiguration()
    {
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("C:\\Users\\alekhya.n\\Desktop\\html\\Task5\\EmployeeManagementSystem\\appSettings.json", optional: true, reloadOnChange: true)
            .Build();
    }

    public static Employee GetEmployeeInputs()
    {
        _logger.DisplayMsg("Employee Number: ");
        int empNo = int.Parse(Console.ReadLine()?? "0" );

        _logger.DisplayMsg("First Name: ");
        string firstName = (Console.ReadLine()?? "");

        _logger.DisplayMsg("Last Name: ");
        string lastName = (Console.ReadLine()?? "");

        _logger.DisplayMsg("Date of Birth (MM-DD-YYYY): ");
        DateTime dob = DateTime.Parse(Console.ReadLine() ?? "0000-00-00");

        _logger.DisplayMsg("Email: ");
        string mail = (Console.ReadLine()?? "");

        _logger.DisplayMsg("Mobile Number: ");
        string mobileNumber = (Console.ReadLine() ?? "0");

        _logger.DisplayMsg("Joining Date (MM-DD-YYYY): ");
        DateTime joiningDate = DateTime.Parse(Console.ReadLine() ?? "0000-00-00");

        _logger.DisplayMsg("Location: (Hyderabad, US, UK)");
        Location Location;
        Enum.TryParse(Console.ReadLine(), ignoreCase: true, out Location);
        
        _logger.DisplayMsg("Department: (PE, IT)");
        Department Department;
        Enum.TryParse(Console.ReadLine(), ignoreCase: true, out Department);
        
        _logger.DisplayMsg("Role: (Intern, Developer, Admin)");
        Role Role;
        Enum.TryParse(Console.ReadLine(), ignoreCase: true, out Role);
        
        _logger.DisplayMsg("Manager: (Hasnu, Sandeep, Bhagvan)");
        Manager Manager;
        Enum.TryParse(Console.ReadLine(), ignoreCase: true, out Manager);
        
        _logger.DisplayMsg("Project: (p1, p2)");
        Project Project;
        Enum.TryParse(Console.ReadLine(), ignoreCase: true, out Project);
        
        Employee employee = new Employee
        {
            EmpNo = empNo,
            FirstName = firstName,
            LastName = lastName,
            Dob = dob,
            Mail = mail,
            MobileNumber = mobileNumber,
            JoiningDate = joiningDate,
            Location = Location,
            Department = Department,
            Role = Role,
            Manager = Manager,
            Project = Project
        };
        return employee;
    }
    
    public static void DisplayEmployee(List<Employee> employees)
    {
        if (employees.Count == 0)
        {
            _logger.LogError("No employees found");
            return;
        }
        foreach (var employee in employees)
        {
            Console.WriteLine(employee.ToString());
        }
        _logger.LogInfo("");
    } 

    public static void FilterAndDisplay()
    {
        _logger.DisplayMsg("Enter Alphabet");
        string alphabetFilter = Console.ReadLine();
    
        _logger.DisplayMsg("Enter Location");
        string locationFilter = Console.ReadLine();
    
        _logger.DisplayMsg("Enter Department");
        string departmentFilter = Console.ReadLine();
    
        _logger.DisplayMsg("Enter EmpNo to search");
        string empNoFilterString = Console.ReadLine(); 
        int? empNoFilter = null;
        if (!string.IsNullOrEmpty(empNoFilterString) && int.TryParse(empNoFilterString, out int empNo))
        {
            empNoFilter = empNo;
        }

        EmployeeFilter filters = new EmployeeFilter
        {
            FirstName = alphabetFilter,
            Location = locationFilter,
            Department = departmentFilter,
            EmpNo = empNoFilter 
        };
        List<Employee> filteredEmployees = _employeeBal.Filter(filters);
        _logger.LogInfo("Employees found : ");
        DisplayEmployee(filteredEmployees);
        _logger.LogSuccess("Filtered Successfully");
    }  

    public static void Update()
    {
        _logger.DisplayMsg("Employee Number: ");
        int empNo = int.Parse(Console.ReadLine()?? "0" );

        _logger.DisplayMsg("First Name: ");
        string firstName = (Console.ReadLine()?? "");

        _logger.DisplayMsg("Last Name: ");
        string lastName = (Console.ReadLine()?? "");

        _logger.DisplayMsg("Date of Birth (MM-DD-YYYY): ");
        DateTime dob = DateTime.Parse(Console.ReadLine() ?? "0000-00-00");

        _logger.DisplayMsg("Email: ");
        string mail = (Console.ReadLine()?? "");

        _logger.DisplayMsg("Mobile Number: ");
        string mobileNumber = (Console.ReadLine() ?? "0");

        _logger.DisplayMsg("Joining Date (MM-DD-YYYY): ");
        DateTime joiningDate = DateTime.Parse(Console.ReadLine() ?? "0000-00-00");

        _logger.DisplayMsg("Location: (Hyderabad, US, UK)");
        Location Location;
        Enum.TryParse(Console.ReadLine(), ignoreCase: true, out Location);
        
        _logger.DisplayMsg("Department: (PE, IT)");
        Department Department;
        Enum.TryParse(Console.ReadLine(), ignoreCase: true, out Department);
        
        _logger.DisplayMsg("Role: (Intern, Developer, Admin)");
        Role Role;
        Enum.TryParse(Console.ReadLine(), ignoreCase: true, out Role);
        
        _logger.DisplayMsg("Manager: (Hasnu, Sandeep, Bhagvan)");
        Manager Manager;
        Enum.TryParse(Console.ReadLine(), ignoreCase: true, out Manager);
        
        _logger.DisplayMsg("Project: (p1, p2)");
        Project Project;
        Enum.TryParse(Console.ReadLine(), ignoreCase: true, out Project);
        
        Employee employee = new Employee
        {
            EmpNo = empNo,
            FirstName = firstName,
            LastName = lastName,
            Dob = dob,
            Mail = mail,
            MobileNumber = mobileNumber,
            JoiningDate = joiningDate,
            Location = Location,
            Department = Department,
            Role = Role,
            Manager = Manager,
            Project = Project
        };
        _employeeBal.Update(employee);
    }
}

public class Options
{
    [Option('a', "add", Required = false, HelpText = "Add an employee")]
    public bool Add { get; set; }
    [Option('d', "display", Required = false, HelpText = "Display all employee info")]
    public bool Display { get; set; }
    [Option('r', "delete", Required = false, HelpText = "Delete employee data")]
    public IEnumerable<int>? Delete { get; set; }
    [Option('e', "edit", Required = false, HelpText = "Edit employee data")]
    public int Edit { get; set; }
    [Option('f', "filter", Required = false, HelpText = "Filter employee data")]
    public bool Filter { get; set; }
}



