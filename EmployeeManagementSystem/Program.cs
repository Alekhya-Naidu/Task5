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
    private static readonly IMasterDataBal _masterDataBAL;
    private static readonly RolesBAL _rolesBAL;
    private static IConfiguration _configuration;
    private static readonly ILogger _logger;
    private static readonly IConsole _console;
    private static readonly IJsonHelper _jsonHelper;
    
    static Program()
    {
        BuildConfiguration();
        _console = new ConsoleWriter();
        _jsonHelper = new JsonHelper();
        _masterDataBAL = new MasterDataBAL(new MasterDataDAl(_configuration, _jsonHelper));
        _rolesBAL = new RolesBAL(new RolesDAL(_configuration, _jsonHelper));    
        _employeeBal = new EmployeeBAL(new EmployeeDAL(_jsonHelper, new MasterDataDAl(_configuration, _jsonHelper), new RolesDAL(_configuration, _jsonHelper), _configuration),_rolesBAL);
    }
    
    public static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args)
        .WithParsed(option =>
        {
            if(option.Add)
            {
                AddEmployee();
            }
            else if(option.Filter)
            {
                FilterAndDisplay();
            }
            else if(option.Edit != 0)
            {
                Update();
            }
            else if (option.Delete != null && option.Delete.Any())
            {
                DeleteEmployee(option);
            }
            else if(option.Display)
            {
                List<Employee> employees = _employeeBal.GetAllEmployees();
                DisplayEmployee(employees);
            }
            else if (option.AddRole)
            {
                AddRole(option);
            }
            else if(option.DisplayRoles)
            {
                DisplayAllRoles();
            }
            else
            {
                _console.PrintError("Invalid Command");
            }
        });
    }

    public static void BuildConfiguration()
    {
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("C:\\Tasks\\Task5\\EmployeeManagementSystem\\appSettings.json", optional: true, reloadOnChange: true)
            .Build();
    } 
    
    private static void AddEmployee()
    {
        try
        {
            if(_employeeBal.Add(GetEmployeeInputs()))
            {
                _console.PrintSuccess("Succesfully Added ");
            }
            else
            {
                _console.PrintError("Invalid");
            }
        }
        catch (Exception ex)    
        {
            _console.PrintError($"Error occured while adding employee : {ex.Message}");
        }
    }
    
    private static void AddRole(Options option)
    {
        try
        {
            if (option.RoleName == null || option.DepartmentId == 0)
            {
                _console.PrintError("Role ID and Department ID are required");
            }
            else
            {
                AddOrUpdateRole(option.DepartmentId, option.RoleName);
                _console.PrintSuccess("Added role "+option.RoleName+" to department");
            }
        }
        catch (Exception ex)
        {
            _console.PrintError($"Error found while adding role : {ex.Message}");
        }
    }
    
    private static void DeleteEmployee(Options option)
    {
        try
        {
            if (_employeeBal.Delete(option.Delete))
            {
                _console.PrintSuccess("Deleted Successfully");
            }
            else
            {
                _console.PrintSuccess("Cannot find Employee");
            }
        }
        catch (Exception ex)
        {
            _console.PrintError($"Error found while deleting : {ex.Message}");
        }
    }

    public static Employee GetEmployeeInputs()
    {
        _console.PrintMsg("Employee Number: ");
        int empNo = int.Parse(Console.ReadLine() ?? "0");

        _console.PrintMsg("First Name: ");
        string firstName = (Console.ReadLine() ?? "");

        _console.PrintMsg("Last Name: ");
        string lastName = (Console.ReadLine() ?? "");

        _console.PrintMsg("Date of Birth (MM-DD-YYYY): ");
        DateTime dob = DateTime.Parse(Console.ReadLine() ?? "0000-00-00");

        _console.PrintMsg("Email: ");
        string mail = (Console.ReadLine() ?? "");

        _console.PrintMsg("Mobile Number: ");
        string mobileNumber = (Console.ReadLine() ?? "0");

        _console.PrintMsg("Joining Date (MM-DD-YYYY): ");
        DateTime joiningDate = DateTime.Parse(Console.ReadLine() ?? "0000-00-00");

        _console.PrintMsg("Location: (Hyderabad, US, UK)");
        string? locationInput = Console.ReadLine()?.Trim().ToLower();
        Location location = _masterDataBAL.GetLocationFromName(locationInput);

        _console.PrintMsg("Department: (PE, IT)");
        string? departmentInput = Console.ReadLine()?.Trim().ToLower();
        Department department = _masterDataBAL.GetDepartmentFromName(departmentInput);
        // if (department == null)
        // {
        //     _console.PrintError("Invalid department selected.");
        //     return null; 
        // }

        _console.PrintMsg("Select Role: ");
        List<Role> roles = _rolesBAL.GetAllRoles();
        foreach (var r in roles)
        {
            if (r.DepartmentId == department.Id)
            {
                _console.PrintMsg(r.Name);
            }
        }
        string? roleInput = Console.ReadLine()?.Trim().ToLower();
        Role role = roles.FirstOrDefault(r => r.Name.ToLower() == roleInput);
        if (role == null)
        {
            _console.PrintError("Invalid role selected.");
            return null;
        }

        _console.PrintMsg("Manager: (Hasnu, Sandeep, Bhagvan)");
        string? managerInput = Console.ReadLine()?.Trim().ToLower();
        Manager manager = _masterDataBAL.GetManagerFromName(managerInput);

        _console.PrintMsg("Project: (p1, p2)");
        string? projectInput = Console.ReadLine()?.Trim().ToLower();
        Project project = _masterDataBAL.GetProjectFromName(projectInput);

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
            _console.PrintError("No employees found");
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

            var location = _masterDataBAL.GetLocationById(employee.LocationId); 

            if (location != null)
            {
                stringBuilder.AppendLine("Location: " + location.Name);
            }
            var department = _masterDataBAL.GetDepartmentById(employee.DepartmentId); 
            if (department != null)
            {
                stringBuilder.AppendLine("Department: " + department.Name);
            }
            var role = _rolesBAL.GetRoleById(employee.RoleId); 
            if (role != null)
            {
                stringBuilder.AppendLine("Role: " + role.Name);
            }
            var manager = _masterDataBAL.GetManagerById(employee.ManagerId); 
            if (manager != null)
            {
                stringBuilder.AppendLine("Manager: " + manager.Name);
            }
            var project = _masterDataBAL.GetProjectById(employee.ProjectId); 
            if (project != null)
            {
                stringBuilder.AppendLine("Project: " + project.Name);
            }
            _console.PrintMsg(stringBuilder.ToString());
        }
    } 

    public static void FilterAndDisplay()
    {
        _console.PrintMsg("Enter Alphabet");
        string alphabetFilter = Console.ReadLine();

        _console.PrintMsg("Enter Location");
        string locationFilter = Console.ReadLine();
        var location = _masterDataBAL.GetLocationFromName(locationFilter);
        int? locationId = location?.Id;

        _console.PrintMsg("Enter Department");
        string departmentFilter = Console.ReadLine();
        var department = _masterDataBAL.GetDepartmentFromName(departmentFilter);
        int? departmentId = department?.Id;

        _console.PrintMsg("Enter EmpNo to search");
        string empNoFilterString = Console.ReadLine(); 
        int? empNoFilter = null;
        if (!string.IsNullOrEmpty(empNoFilterString) && int.TryParse(empNoFilterString, out int empNo))
        {
            empNoFilter = empNo;
        }  
        
        EmployeeFilter filters = new EmployeeFilter
        {
            FirstName = alphabetFilter,
            LocationId = locationId,
            DepartmentId = departmentId,
            EmpNo = empNoFilter 
        };
        
        List<Employee> filteredEmployees = _employeeBal.Filter(filters);
        _console.PrintMsg("Employees found : ");
        DisplayEmployee(filteredEmployees);
        _console.PrintSuccess("Filtered Successfully");
    }

    public static void Update()
    {
        _console.PrintMsg("Employee Number: ");
        int empNo = int.Parse(Console.ReadLine()?? "0" );

        Employee existingEmployee = _employeeBal.GetAllEmployees().FirstOrDefault(emp => emp.EmpNo == empNo);
        if (existingEmployee == null)
        {
            _console.PrintError("Employee not found.");
            return;
        }

        _console.PrintMsg("First Name: ");
        string firstName = (Console.ReadLine()?? "");

        _console.PrintMsg("Last Name: ");
        string lastName = (Console.ReadLine()?? "");

        _console.PrintMsg("Date of Birth (MM-DD-YYYY): ");
        DateTime dob = DateTime.Parse(Console.ReadLine() ?? "0000-00-00");

        _console.PrintMsg("Email: ");
        string mail = (Console.ReadLine()?? "");

        _console.PrintMsg("Mobile Number: ");
        string mobileNumber = (Console.ReadLine() ?? "0");

        _console.PrintMsg("Joining Date (MM-DD-YYYY): ");
        DateTime joiningDate = DateTime.Parse(Console.ReadLine() ?? "0000-00-00");

        _console.PrintMsg("Location: (Hyderabad, US, UK)");
        string? locationInput = Console.ReadLine()?.Trim().ToLower();
        Location location = _masterDataBAL.GetLocationFromName(locationInput);

        _console.PrintMsg("Department: (PE, IT)");
        string? departmentInput = Console.ReadLine()?.Trim().ToLower();
        Department department = _masterDataBAL.GetDepartmentFromName(departmentInput);

        _console.PrintMsg("Role: (Intern, Developer, Admin)");
        string? roleInput = Console.ReadLine()?.Trim().ToLower();
        Role role = _rolesBAL.GetRoleFromName(roleInput);

        _console.PrintMsg("Manager: (Hasnu, Sandeep, Bhagvan)");
        string? managerInput = Console.ReadLine()?.Trim().ToLower();
        Manager manager = _masterDataBAL.GetManagerFromName(managerInput);

        _console.PrintMsg("Project: (p1, p2)");
        string? projectInput = Console.ReadLine()?.Trim().ToLower();
        Project project = _masterDataBAL.GetProjectFromName(projectInput);

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
            _rolesBAL.UpdateRoles(roles);
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
