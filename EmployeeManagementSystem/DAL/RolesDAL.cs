using System.IO;
using System.Collections.Generic;
using System.Text.Json;

namespace EmployeeManagement;

public class RolesDAL : IRolesDAL
{
    private readonly string _roleFilePath;
    private readonly string _departmentFilePath;
    public RolesDAL(string roleFilePath, string departmentFilePath)
    {
        _roleFilePath = roleFilePath;
        _departmentFilePath = departmentFilePath;
    }

    public List<Role> GetAllRoles()
    {
        List<Role> existingRoles = new List<Role>();
        if (File.Exists(_roleFilePath))
        {
            string json = File.ReadAllText(_roleFilePath);
            existingRoles = JsonSerializer.Deserialize<List<Role>>(json) ?? new List<Role>();
        }
        return existingRoles;
    }

    public Role GetRoleById(int roleId)
    {
        try
        {
            List<Role> roles = LoadData<Role>(_roleFilePath);
            return roles.FirstOrDefault(role => role.Id == roleId);
        }
        catch
        {
            return null;
        }
    }

    private List<T> LoadData<T>(string filePath)
    {
        try
        {
            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<T>>(json);
        }
        catch
        {
            return new List<T>();
        }
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
        UpdateRolesFile(roles);
        return true;
    }

    public bool UpdateRolesFile(List<Role> roles)
    {
        try
        {
            string json = JsonSerializer.Serialize(roles, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_roleFilePath, json);
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
        if (File.Exists(_departmentFilePath))
        {
            string json = File.ReadAllText(_departmentFilePath);
            existingDepartments = JsonSerializer.Deserialize<List<Department>>(json) ?? new List<Department>();
        }
        return existingDepartments;
    }
}
