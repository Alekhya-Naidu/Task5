using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace EmployeeManagement;

public class RolesDAL : IRolesDAL
{
    private readonly IConfiguration _configuration;
    private readonly IJsonHelper _jsonHelper;
    
    public RolesDAL(IConfiguration configuration, IJsonHelper jsonHelper)
    {
        _configuration = configuration;
        _jsonHelper = jsonHelper;
    }

    public List<Role> GetAllRoles()
    {
        List<Role> existingRoles = new List<Role>();
        if (File.Exists(Path.Combine(_configuration?["BaseFilePath"],_configuration?["RoleDataFilePath"])))
        {
            string json = _jsonHelper.ReadFromFile(Path.Combine(_configuration?["BaseFilePath"],_configuration?["RoleDataFilePath"]));
            existingRoles = _jsonHelper.Deserialize<List<Role>>(json);
        }
        return existingRoles;
    }

    public Role GetRoleFromName(string roleInput)
    {
        List<Role> roles = GetAllRoles();
        foreach (var role in roles)
        {
            if (string.Equals(role.Name.ToLower(), roleInput, StringComparison.OrdinalIgnoreCase))
            {
                return role;
            }
        }
        return null;
    }
    
    public Role GetRoleById(int roleId)
    {
        List<Role> roles = GetAllRoles();
        return roles.FirstOrDefault(role => role.Id == roleId);
    }

    public int GetRoleId(Role role)
    {
        if(role != null)
        {
            if(role.Id != 0)
            {
                return role.Id;
            }
            else if(!string.IsNullOrEmpty(role.Name))
            {
                Role existingRole =  GetRoleFromName(role.Name);
                return existingRole?.Id ?? 0;
            }
        }
        return 0;
    }
    
    public bool AddRole(int departmentId, int roleId, string roleName)
    {
        var roles = GetAllRoles();
        if (roles.Any(r => r.DepartmentId == departmentId && r.Id == roleId))
        {
            return false;
        }
        roles.Add(new Role
        {
            DepartmentId = departmentId,
            Id = roleId,
            Name = roleName
        });
        UpdateRoles(roles);
        return true;
    }

    public bool UpdateRoles(List<Role> roles)
    {
        try
        {
            string json = _jsonHelper.Serialize(roles);
            File.WriteAllText(Path.Combine(_configuration?["BaseFilePath"],_configuration?["RoleDataFilePath"]), json);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public List<Department> GetAllDepartments()
    {
        List<Department> existingDepartments = new List<Department>();
        if (File.Exists(Path.Combine(_configuration?["BaseFilePath"], _configuration?["DepartmentDataFilePath"])))
        {

            string json = File.ReadAllText(Path.Combine(_configuration?["BaseFilePath"], _configuration?["DepartmentDataFilePath"]));
            existingDepartments = JsonSerializer.Deserialize<List<Department>>(json) ?? new List<Department>();
        }
        return existingDepartments;
    }
}
