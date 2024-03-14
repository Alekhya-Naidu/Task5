using System;
using CommandLine;
using System.Text.Json;

namespace EmployeeManagement;
class Program
{   
    private readonly string fileName = "EmployeesData.json";
    private readonly ILogger ilogger;
    private readonly EmployeeManagementSystem.Employee empModel = new EmployeeManagementSystem.Employee();
    public Program(ILogger logger)
    {
        ilogger = logger;
    }
    public static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args)
        .WithParsed(options =>
        {
            ILogger log = new LoggerHandler();
            Program program = new Program(log);
            EmployeeManager empManager = new EmployeeManager(log);
            EmployeeDataAccess empAccess = new EmployeeDataAccess(log);
            if(options.Add)
            {
                empManager.AddEmployee(log);
            }
            else if(options.Display)
            {
                program.DisplayAllEmployees();
            }
            else if(options.Delete != null && options.Delete.Any())
            {
                empManager.DeleteEmployee(options.Delete.ToArray());
            }
            else if(options.Search != 0)
            {
                empAccess.FindEmployee(options.Search, log);
            }
            else if(options.Edit != 0)
            {
                empAccess.EditEmployee(options.Edit, log);
            }
            else if(options.Filter)
            {
                empAccess.FilterEmployeeDetails();
            }
            else
            {
                Console.WriteLine("Invalid Command");
            }
        });
    }
    public EmployeeManagementSystem.Employee EmployeeDataInput()
    {
        ilogger.LogMsg("Enter employee details:");
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
        if(!(mail.Contains("@") && mail.Contains(".")))
        {
            ilogger.LogMsg("Enter valid Mail");
            return new EmployeeManagementSystem.Employee();
        }

        ilogger.LogMsg("Mobile Number: ");
        long mobileNumber = long.Parse(Console.ReadLine() ?? "000000000");
        if(Math.Floor(Math.Log10(mobileNumber) + 1) != 10)
        {
            ilogger.LogMsg("Enter valid Mobile Number");
            return new EmployeeManagementSystem.Employee();
        }

        ilogger.LogMsg("Joining Date (MM-DD-YYYY): ");
        DateTime joiningDate = DateTime.Parse(Console.ReadLine() ?? "0000-00-00");

        ilogger.LogMsg("Location: (Hyderabad, US, UK)");
        if (!Enum.TryParse<EmployeeManagementSystem.Location>(Console.ReadLine(), ignoreCase: true, out EmployeeManagementSystem.Location Location))
        {
            ilogger.LogMsg("Invalid : Choose Location from avaiable options");
            return new EmployeeManagementSystem.Employee();
        }
        
        ilogger.LogMsg("Department: (PE, IT)");
        if (!Enum.TryParse<EmployeeManagementSystem.Department>(Console.ReadLine(), ignoreCase: true, out EmployeeManagementSystem.Department Department))
        {
            ilogger.LogMsg("Invalid : Choose Department from avaiable options");
            return new EmployeeManagementSystem.Employee();
        }
        
        ilogger.LogMsg("Role: (Intern, Developer, Admin)");
        string userInput = Console.ReadLine();
        if (!Enum.TryParse<EmployeeManagementSystem.Role>(userInput, ignoreCase: true, out EmployeeManagementSystem.Role Role))
        {
            ilogger.LogMsg("Invalid: Choose Role from available options");
            return new EmployeeManagementSystem.Employee();
        }
        
        ilogger.LogMsg("Manager: (Hasnu, Sandeep, Bhagvan)");
        if (!Enum.TryParse<EmployeeManagementSystem.Manager>(Console.ReadLine(), ignoreCase: true, out EmployeeManagementSystem.Manager Manager))
        {
            ilogger.LogMsg("Invalid : Choose Manager from avaiable options");
            return new EmployeeManagementSystem.Employee();
        }
        
        ilogger.LogMsg("Project: (p1, p2)");
        if (!Enum.TryParse<EmployeeManagementSystem.Project>(Console.ReadLine(), ignoreCase: true, out EmployeeManagementSystem.Project Project))
        {
            ilogger.LogMsg("Invalid : Choose Project from avaiable options");
            return new EmployeeManagementSystem.Employee();
        }

        EmployeeManagementSystem.Employee employee = new EmployeeManagementSystem.Employee
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
    public EmployeeManagementSystem.Employee UpdateFromInput(EmployeeManagementSystem.Employee updateEmployee)
    {
        ilogger.LogMsg("Enter updated employee details:");
        
        ilogger.LogMsg("Employee Number: ");
        updateEmployee.EmpNo = int.Parse(Console.ReadLine());

        ilogger.LogMsg("First Name: ");
        updateEmployee.FirstName = Console.ReadLine();

        ilogger.LogMsg("Last Name: ");
        updateEmployee.LastName = Console.ReadLine();

        ilogger.LogMsg("Date of Birth (MM-DD-YYYY): ");
        updateEmployee.Dob = DateTime.Parse(Console.ReadLine());

        ilogger.LogMsg("Email: ");
        string mail = Console.ReadLine();
        if (!(mail.Contains("@") && mail.Contains(".")))
        {
            ilogger.LogMsg("Enter valid Mail");
            return null; 
        }
        updateEmployee.Mail = mail;

        ilogger.LogMsg("Mobile Number: ");
        long mobileNumber = long.Parse(Console.ReadLine());
        if (Math.Floor(Math.Log10(mobileNumber) + 1) != 10)
        {
            ilogger.LogMsg("Enter valid Mobile Number");
            return null;
        }
        updateEmployee.MobileNumber = mobileNumber;

        ilogger.LogMsg("Joining Date (MM-DD-YYYY): ");
        updateEmployee.JoiningDate = DateTime.Parse(Console.ReadLine());

        ilogger.LogMsg("Location: (Hyderabad, US, UK)");
        if (!Enum.TryParse<EmployeeManagementSystem.Location>(Console.ReadLine(), ignoreCase: true, out EmployeeManagementSystem.Location location))
        {
            ilogger.LogMsg("Invalid : Choose Location from available options");
            return null; 
        }
        updateEmployee.Location = location;

        ilogger.LogMsg("Department: (PE, IT)");
        if (!Enum.TryParse<EmployeeManagementSystem.Department>(Console.ReadLine(), ignoreCase: true, out EmployeeManagementSystem.Department department))
        {
            ilogger.LogMsg("Invalid : Choose Department from available options");
            return null; 
        }
        updateEmployee.Department = department;

        ilogger.LogMsg("Role: (Intern, Developer, Admin)");
        if (!Enum.TryParse<EmployeeManagementSystem.Role>(Console.ReadLine(), ignoreCase: true, out EmployeeManagementSystem.Role role))
        {
            ilogger.LogMsg("Invalid: Choose Role from available options");
            return null;
        }
        updateEmployee.Role = role;

        ilogger.LogMsg("Manager: (Hasnu, Sandeep, Bhagvan)");
        if (!Enum.TryParse<EmployeeManagementSystem.Manager>(Console.ReadLine(), ignoreCase: true, out EmployeeManagementSystem.Manager manager))
        {
            ilogger.LogMsg("Invalid : Choose Manager from available options");
            return null;
        }
        updateEmployee.Manager = manager;

        ilogger.LogMsg("Project: (p1, p2)");
        if (!Enum.TryParse<EmployeeManagementSystem.Project>(Console.ReadLine(), ignoreCase: true, out EmployeeManagementSystem.Project project))
        {
            ilogger.LogMsg("Invalid : Choose Project from available options");
            return null; 
        }
        updateEmployee.Project = project;

        return updateEmployee;
    }
    public void DisplayAllEmployees()
    {

        if (File.Exists(fileName))
        {
            string jsonData = File.ReadAllText(fileName);
            List<EmployeeManagementSystem.Employee>? employees = JsonSerializer.Deserialize<List<EmployeeManagementSystem.Employee>>(jsonData);
            if (employees != null && employees.Count > 0)
            {
                ilogger.LogSuccessful("All Employees:");
                foreach (var emp in employees)
                {
                    ilogger.LogMsg(emp.ToString());
                    ilogger.LogMsg("");
                }
            }
            else
            {
                ilogger.LogError("No employees found.");
            }
        }
        else
        {
            ilogger.LogError("Employee data file not found.");
        }
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
    [Option('s', "search", Required = false, HelpText = "Search for an employee")]
    public int Search { get; set; }
    [Option('e', "edit", Required = false, HelpText = "Edit employee data")]
    public int Edit { get; set; }
    [Option('f', "filter", Required = false, HelpText = "Filter employee data")]
    public bool Filter { get; set; }
}



