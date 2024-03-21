using System;
using CommandLine;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace EmployeeManagement;

public static class Program
{   
    private static readonly IEmployeeBAL _employeeBal;
    private static IConfiguration _configuration;
    private static readonly ILogger _logger;
    private static readonly string _filePath;
    private static readonly string _locationFilePath;
    private static readonly string _departmentFilePath;
    private static readonly string _roleFilePath;
    private static readonly string _managerFilePath;
    private static readonly string _projectFilePath;
    private static RolesBAL _rolesBAL;
    
    static Program()
    {
        BuildConfiguration();
        _logger = new ConsoleLogger();
        _filePath = _configuration?["EmployeeDataFilePath"] ?? "defaultFilePath";
        _locationFilePath = _configuration?["LocationDataFilePath"];
        _departmentFilePath = _configuration?["DepartmentDataFilePath"];
        _roleFilePath = _configuration?["RoleDataFilePath"];
        _managerFilePath = _configuration?["ManagerDataFilePath"];
        _projectFilePath = _configuration?["ProjectDataFilePath"];
        _rolesBAL = new RolesBAL(new RolesDAL(_roleFilePath, _departmentFilePath));    
        _employeeBal = new EmployeeBAL(new EmployeeDAL(_logger, _filePath, _locationFilePath, _departmentFilePath, _roleFilePath, _managerFilePath, _projectFilePath),_rolesBAL, _logger);
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
           else if (options.AddRole)
            {
                if (options.RoleName == null || options.DepartmentId == 0)
                {
                    _logger.LogError("Role ID and Department ID are required");
                }
                else
                {
                    AddOrUpdateRole(options.DepartmentId, options.RoleName);
                    _logger.LogSuccess("Added role "+options.RoleName+" to department");
                }
            }
            else if(options.DisplayRoles)
            {
                DisplayAllRoles();
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
        int empNo = int.Parse(Console.ReadLine() ?? "0");

        _logger.DisplayMsg("First Name: ");
        string firstName = (Console.ReadLine() ?? "");

        _logger.DisplayMsg("Last Name: ");
        string lastName = (Console.ReadLine() ?? "");

        _logger.DisplayMsg("Date of Birth (MM-DD-YYYY): ");
        DateTime dob = DateTime.Parse(Console.ReadLine() ?? "0000-00-00");

        _logger.DisplayMsg("Email: ");
        string mail = (Console.ReadLine() ?? "");

        _logger.DisplayMsg("Mobile Number: ");
        string mobileNumber = (Console.ReadLine() ?? "0");

        _logger.DisplayMsg("Joining Date (MM-DD-YYYY): ");
        DateTime joiningDate = DateTime.Parse(Console.ReadLine() ?? "0000-00-00");

        _logger.DisplayMsg("Location: (Hyderabad, US, UK)");
        string? locationInput = Console.ReadLine()?.Trim().ToLower();
        Location location = _employeeBal.GetLocationFromInput(locationInput);

        _logger.DisplayMsg("Department: (PE, IT)");
        string? departmentInput = Console.ReadLine()?.Trim().ToLower();
        Department department = _employeeBal.GetDepartmentFromInput(departmentInput);
        if (department == null)
        {
            _logger.LogError("Invalid department selected.");
            return null; 
        }

        _logger.DisplayMsg("Select Role: ");
        List<Role> roles = _rolesBAL.GetAllRoles();
        foreach (var r in roles)
        {
            if (r.DepartmentId == department.Id)
            {
                _logger.DisplayMsg(r.Name);
            }
        }
        string? roleInput = Console.ReadLine()?.Trim().ToLower();
        Role role = roles.FirstOrDefault(r => r.Name.ToLower() == roleInput);
        if (role == null)
        {
            _logger.LogError("Invalid role selected.");
            return null;
        }

        _logger.DisplayMsg("Manager: (Hasnu, Sandeep, Bhagvan)");
        string? managerInput = Console.ReadLine()?.Trim().ToLower();
        Manager manager = _employeeBal.GetManagerFromInput(managerInput);

        _logger.DisplayMsg("Project: (p1, p2)");
        string? projectInput = Console.ReadLine()?.Trim().ToLower();
        Project project = _employeeBal.GetProjectFromInput(projectInput);

        Employee employee = new Employee
        {
            EmpNo = empNo,
            FirstName = firstName,
            LastName = lastName,
            Dob = dob,
            Mail = mail,
            MobileNumber = mobileNumber,
            JoiningDate = joiningDate,
            LocationId = location?.Id ?? 0,
            DepartmentId = department?.Id ?? 0,
            RoleId = role?.Id ?? 0,
            ManagerId = manager?.Id ?? 0,
            ProjectId = project?.Id ?? 0,
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
        foreach(var employee in employees)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("EmpNo : " + employee.EmpNo);
            stringBuilder.AppendLine("Name : " + employee.FirstName + " " + employee.LastName);
            stringBuilder.AppendLine("DOB : " + employee.Dob);
            stringBuilder.AppendLine("Mail : " + employee.Mail);
            stringBuilder.AppendLine("MobileNumber : " + employee.MobileNumber);
            stringBuilder.AppendLine("JoiningDate : " + employee.JoiningDate);

            var location = _employeeBal.GetLocationById(employee.LocationId); 
            if (location != null)
            {
                stringBuilder.AppendLine("Location: " + location.Name);
            }
            var department = _employeeBal.GetDepartmentById(employee.DepartmentId); 
            if (department != null)
            {
                stringBuilder.AppendLine("Department: " + department.Name);
            }
            var role = _employeeBal.GetRoleById(employee.RoleId); 
            if (role != null)
            {
                stringBuilder.AppendLine("Role: " + role.Name);
            }
            var manager = _employeeBal.GetManagerById(employee.ManagerId); 
            if (manager != null)
            {
                stringBuilder.AppendLine("Manager: " + manager.Name);
            }
            var project = _employeeBal.GetProjectById(employee.ProjectId); 
            if (project != null)
            {
                stringBuilder.AppendLine("Project: " + project.Name);
            }
            _logger.LogInfo(stringBuilder.ToString());
        }
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
            LocationName = locationFilter,
            DepartmentName = departmentFilter,
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

        Employee existingEmployee = _employeeBal.GetAllEmployees().FirstOrDefault(emp => emp.EmpNo == empNo);
        if (existingEmployee == null)
        {
            _logger.LogError("Employee not found.");
            return;
        }

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
        string? locationInput = Console.ReadLine()?.Trim().ToLower();
        Location location = _employeeBal.GetLocationFromInput(locationInput);

        _logger.DisplayMsg("Department: (PE, IT)");
        string? departmentInput = Console.ReadLine()?.Trim().ToLower();
        Department department = _employeeBal.GetDepartmentFromInput(departmentInput);

        _logger.DisplayMsg("Role: (Intern, Developer, Admin)");
        string? roleInput = Console.ReadLine()?.Trim().ToLower();
        Role role = _employeeBal.GetRoleFromInput(roleInput);

        _logger.DisplayMsg("Manager: (Hasnu, Sandeep, Bhagvan)");
        string? managerInput = Console.ReadLine()?.Trim().ToLower();
        Manager manager = _employeeBal.GetManagerFromInput(managerInput);

        _logger.DisplayMsg("Project: (p1, p2)");
        string? projectInput = Console.ReadLine()?.Trim().ToLower();
        Project project = _employeeBal.GetProjectFromInput(projectInput);

        Employee employee = new Employee
        {
            EmpNo = empNo,
            FirstName = firstName,
            LastName = lastName,
            Dob = dob,
            Mail = mail,
            MobileNumber = mobileNumber,
            JoiningDate = joiningDate,
            LocationId = location?.Id ?? 0,
            DepartmentId = department?.Id ?? 0,
            RoleId = role?.Id ?? 0,
            ManagerId = manager?.Id ?? 0,
            ProjectId = project?.Id ?? 0,
        };
        _employeeBal.Update(employee);
    }

    private static void DisplayAllRoles()
    {
        List<Role> roles = _rolesBAL.GetAllRoles();
        foreach (var role in roles)
        {
            Console.WriteLine("Dept ID : "+role.DepartmentId+"\t Role ID: "+role.Id+ "\tName: "+role.Name);
        }
    }

    public static bool AddOrUpdateRole(int departmentId, string roleName)
    {
        try
        {
            List<Department> departments = _rolesBAL.GetAllDepartments(); 
            Department department = departments.Find(d => d.Id == departmentId);
            if (department == null)
            {
                return false;

            }
            List<Role> roles = _rolesBAL.GetAllRoles();
            int roleId = roles.Count + 1; 
            Role role = new Role
            {
                Id = roleId,
                Name = roleName,
                DepartmentId = departmentId
            };
            roles.Add(role);
            _rolesBAL.UpdateRolesFile(roles);
            return true;
        }
        catch
        {
            return false;
        }
    }
}

public class Options
{
    [Option('a', "add", Required = false, HelpText = "Add an employee")]
    public bool Add { get; set; }
    [Option('s', "display", Required = false, HelpText = "Display all employee info")]
    public bool Display { get; set; }
    [Option('r', "delete", Required = false, HelpText = "Delete employee data")]
    public IEnumerable<int>? Delete { get; set; }
    [Option('e', "edit", Required = false, HelpText = "Edit employee data")]
    public int Edit { get; set; }
    [Option('f', "filter", Required = false, HelpText = "Filter employee data")]
    public bool Filter { get; set; }
    [Option('p', "addrole", Required = false, HelpText = "Addition of roles to department")]
    public bool AddRole { get; set; }
    [Option('j', "displayroles", Required = false, HelpText = "Shows roles available to a department")]
    public bool DisplayRoles { get; set; }
    [Option('r', "roleId", Required = false, HelpText = "Role ID")]
    public int RoleId { get; set; }
    [Option('d', "departmentId", Required = false, HelpText = "Department ID")]
    public int DepartmentId { get; set; }
    [Option('n', "roleName", Required = false, HelpText = "Role Name")]
    public string RoleName { get; set; }
}
