using System;
using CommandLine;
using System.Collections.Generic;
using System.Text.Json;

namespace EmployeeManagement;

class Program
{   
    private readonly IBAL _bal;
    private readonly ILogger _console;
    private readonly string filePath = "";
    public Program(IBAL bal, ILogger console)
    {
        _bal = bal;
        _console = console;
    }
    
    public static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args)
        .WithParsed(options =>
        {
            ILogger console = new ConsoleLogger();
            IBAL bal = new EmployeeBAL(new EmployeeDAL(console), console);
            Program program = new Program(bal, console);
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
                program._console.LogError("Invalid Command");
            }
        });
    }

    public Employee EmployeeDataInput()
    {
        _console.LogInfo("Employee Number: ");
        int empNo = int.Parse(Console.ReadLine()?? "0" );

        _console.LogInfo("First Name: ");
        string firstName = (Console.ReadLine()?? "");

        _console.LogInfo("Last Name: ");
        string lastName = (Console.ReadLine()?? "");

        _console.LogInfo("Date of Birth (MM-DD-YYYY): ");
        DateTime dob = DateTime.Parse(Console.ReadLine() ?? "0000-00-00");

        _console.LogInfo("Email: ");
        string mail = (Console.ReadLine()?? "");

        _console.LogInfo("Mobile Number: ");
        string mobileNumber = (Console.ReadLine() ?? "0");

        _console.LogInfo("Joining Date (MM-DD-YYYY): ");
        DateTime joiningDate = DateTime.Parse(Console.ReadLine() ?? "0000-00-00");

        _console.LogInfo("Location: (Hyderabad, US, UK)");
        Location Location;
        Enum.TryParse(Console.ReadLine(), ignoreCase: true, out Location);
        
        _console.LogInfo("Department: (PE, IT)");
        Department Department;
        Enum.TryParse(Console.ReadLine(), ignoreCase: true, out Department);
        
        _console.LogInfo("Role: (Intern, Developer, Admin)");
        Role Role;
        Enum.TryParse(Console.ReadLine(), ignoreCase: true, out Role);
        
        _console.LogInfo("Manager: (Hasnu, Sandeep, Bhagvan)");
        Manager Manager;
        Enum.TryParse(Console.ReadLine(), ignoreCase: true, out Manager);
        
        _console.LogInfo("Project: (p1, p2)");
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
            _console.LogError("No employees found");
            return;
        }
        foreach (var employee in employees)
        {
            Console.WriteLine(employee.ToString());
        }
    } 

    public void FilterAndDisplay()
    {
        _console.LogInfo("Enter Alphabet");
        string alphabetFilter = Console.ReadLine();
    
        _console.LogInfo("Enter Location");
        string locationFilter = Console.ReadLine();
    
        _console.LogInfo("Enter Department");
        string departmentFilter = Console.ReadLine();
    
        _console.LogInfo("Enter EmpNo to search");
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
        _console.LogInfo("Employee Number: ");
        int empNo = int.Parse(Console.ReadLine()?? "0" );

        _console.LogInfo("First Name: ");
        string firstName = (Console.ReadLine()?? "");

        _console.LogInfo("Last Name: ");
        string lastName = (Console.ReadLine()?? "");

        _console.LogInfo("Date of Birth (MM-DD-YYYY): ");
        DateTime dob = DateTime.Parse(Console.ReadLine() ?? "0000-00-00");

        _console.LogInfo("Email: ");
        string mail = (Console.ReadLine()?? "");

        _console.LogInfo("Mobile Number: ");
        string mobileNumber = (Console.ReadLine() ?? "0");

        _console.LogInfo("Joining Date (MM-DD-YYYY): ");
        DateTime joiningDate = DateTime.Parse(Console.ReadLine() ?? "0000-00-00");

        _console.LogInfo("Location: (Hyderabad, US, UK)");
        Location Location;
        Enum.TryParse(Console.ReadLine(), ignoreCase: true, out Location);
        
        _console.LogInfo("Department: (PE, IT)");
        Department Department;
        Enum.TryParse(Console.ReadLine(), ignoreCase: true, out Department);
        
        _console.LogInfo("Role: (Intern, Developer, Admin)");
        Role Role;
        Enum.TryParse(Console.ReadLine(), ignoreCase: true, out Role);
        
        _console.LogInfo("Manager: (Hasnu, Sandeep, Bhagvan)");
        Manager Manager;
        Enum.TryParse(Console.ReadLine(), ignoreCase: true, out Manager);
        
        _console.LogInfo("Project: (p1, p2)");
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



