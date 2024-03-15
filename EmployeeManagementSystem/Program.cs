using System;
using CommandLine;
using System.Collections.Generic;
using System.Text.Json;

namespace EmployeeManagement;

class Program
{   
    private readonly IBAL _bal;
    private readonly ILogger ilogger;
    private readonly string filePath = "";
    public Program(IBAL bal, ILogger logger)
    {
        _bal = bal;
        ilogger = logger;
    }
    
    public static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args)
        .WithParsed(options =>
        {
            ILogger logger = new ConsoleLogger();
            IBAL bal = new EmployeeBAL(new EmployeeDAL(logger), logger);
            Program program = new Program(bal, logger);
            if(options.Add)
            {
                program._bal.Add(program.EmployeeDataInput());
            }
            else if(options.Filter)
            {
                program.FilterAndDisplay();
            }
            else if(options.Edit != 0)
            {
                program._bal.Update(program.Update());
            }
            else if (options.Delete != null && options.Delete.Any())
            {
                program._bal.Delete(options.Delete);
            }
            else if(options.Display)
            {
                List<Employee> employees = program._bal.GetAllEmployees();
                program.Display(employees);
            }
            else
            {
                program.ilogger.LogError("Invalid Command");
            }
        });
    }

    public Employee EmployeeDataInput()
    {
        ilogger.LogMsg("Employee Number: ");
        int empNo = int.Parse(Console.ReadLine()?? "0" );

        ilogger.LogMsg("First Name: ");
        string firstName = (Console.ReadLine()?? "");

        ilogger.LogMsg("Last Name: ");
        string lastName = (Console.ReadLine()?? "");

        ilogger.LogMsg("Date of Birth (MM-DD-YYYY): ");
        DateTime dob = DateTime.Parse(Console.ReadLine() ?? "0000-00-00");

        ilogger.LogMsg("Email: ");
        string mail = (Console.ReadLine()?? "");

        ilogger.LogMsg("Mobile Number: ");
        long mobileNumber = long.Parse(Console.ReadLine() ?? "0");

        ilogger.LogMsg("Joining Date (MM-DD-YYYY): ");
        DateTime joiningDate = DateTime.Parse(Console.ReadLine() ?? "0000-00-00");

        ilogger.LogMsg("Location: (Hyderabad, US, UK)");
        Location Location;
        Enum.TryParse(Console.ReadLine(), ignoreCase: true, out Location);
        
        ilogger.LogMsg("Department: (PE, IT)");
        // if (!Enum.TryParse<Department>(Console.ReadLine(), ignoreCase: true, out Department Department))
        // {
        //     ilogger.LogMsg("Invalid : Choose Department from avaiable options");
        //     return new Employee();
        // }
        Department Department;
        Enum.TryParse(Console.ReadLine(), ignoreCase: true, out Department);
        
        ilogger.LogMsg("Role: (Intern, Developer, Admin)");
        Role Role;
        Enum.TryParse(Console.ReadLine(), ignoreCase: true, out Role);
        
        ilogger.LogMsg("Manager: (Hasnu, Sandeep, Bhagvan)");
        Manager Manager;
        Enum.TryParse(Console.ReadLine(), ignoreCase: true, out Manager);
        
        ilogger.LogMsg("Project: (p1, p2)");
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
    
    public void Display(List<Employee> employees)
    {
        if (employees.Count == 0)
        {
            ilogger.LogError("No employees found");
            return;
        }
        foreach (var employee in employees)
        {
            Console.WriteLine(employee.ToString());
        }
    } 

    public void FilterAndDisplay()
    {
        ilogger.LogMsg("Enter Alphabet");
        string alphabetFilter = Console.ReadLine();
    
        ilogger.LogMsg("Enter Location");
        string locationFilter = Console.ReadLine();
    
        ilogger.LogMsg("Enter Department");
        string departmentFilter = Console.ReadLine();
    
        ilogger.LogMsg("Enter EmpNo to search");
        string empNoFilter = Console.ReadLine();

        List<string> filters = new List<string>();
        if (!string.IsNullOrEmpty(alphabetFilter))
            filters.Add(alphabetFilter.ToLower()); 
        if (!string.IsNullOrEmpty(locationFilter))
            filters.Add(locationFilter.ToLower());
        if (!string.IsNullOrEmpty(departmentFilter))
            filters.Add(departmentFilter.ToLower());
        if (!string.IsNullOrEmpty(empNoFilter))
            filters.Add(empNoFilter.ToLower());

        List<Employee> filteredEmployees = _bal.Filter(filters);
        Display(filteredEmployees);
    }  

    public Employee Update()
    {
        ilogger.LogMsg("Employee Number: ");
        int empNo = int.Parse(Console.ReadLine()?? "0" );

        ilogger.LogMsg("First Name: ");
        string firstName = (Console.ReadLine()?? "");

        ilogger.LogMsg("Last Name: ");
        string lastName = (Console.ReadLine()?? "");

        ilogger.LogMsg("Date of Birth (MM-DD-YYYY): ");
        DateTime dob = DateTime.Parse(Console.ReadLine() ?? "0000-00-00");

        ilogger.LogMsg("Email: ");
        string mail = (Console.ReadLine()?? "");

        ilogger.LogMsg("Mobile Number: ");
        long mobileNumber = long.Parse(Console.ReadLine() ?? "0");

        ilogger.LogMsg("Joining Date (MM-DD-YYYY): ");
        DateTime joiningDate = DateTime.Parse(Console.ReadLine() ?? "0000-00-00");

        ilogger.LogMsg("Location: (Hyderabad, US, UK)");
        Location Location;
        Enum.TryParse(Console.ReadLine(), ignoreCase: true, out Location);
        
        ilogger.LogMsg("Department: (PE, IT)");
        Department Department;
        Enum.TryParse(Console.ReadLine(), ignoreCase: true, out Department);
        
        ilogger.LogMsg("Role: (Intern, Developer, Admin)");
        Role Role;
        Enum.TryParse(Console.ReadLine(), ignoreCase: true, out Role);
        
        ilogger.LogMsg("Manager: (Hasnu, Sandeep, Bhagvan)");
        Manager Manager;
        Enum.TryParse(Console.ReadLine(), ignoreCase: true, out Manager);
        
        ilogger.LogMsg("Project: (p1, p2)");
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



