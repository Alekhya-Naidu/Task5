using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using EMS.DAL.DBO;
using EMS.DAL.Interfaces;  
using EMS.Common.Helpers;

namespace EMS.DAL.DAL;

public class RolesDAL : IRolesDAL
{
    private readonly IConfiguration _configuration;
    private readonly IJsonHelper _jsonHelper;
    
    public RolesDAL(IConfiguration configuration, IJsonHelper jsonHelper)
    {
        _configuration = configuration;
        _jsonHelper = jsonHelper;
    }

    public List<Role> GetAllRoles<Role>(string rolefilePath)
    {
        try
        {
            string json = _jsonHelper.ReadFromFile(rolefilePath);
            return _jsonHelper.Deserialize<List<Role>>(json);
        }
        catch
        {
            return new List<Role>();
        }
    }

    public Role GetRoleFromName(string roleInput)
    {
        List<Role> roles = GetAllRoles<Role>(Path.Combine(_configuration?["BaseFilePath"],_configuration?["RoleDataFilePath"]));
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
        List<Role> roles = GetAllRoles<Role>(Path.Combine(_configuration?["BaseFilePath"],_configuration?["RoleDataFilePath"]));
        return roles.FirstOrDefault(role => role.Id == roleId);
    }
    
    public bool AddRole(int departmentId, int roleId, string roleName)
    {
        var roles = GetAllRoles<Role>(Path.Combine(_configuration?["BaseFilePath"],_configuration?["RoleDataFilePath"]));
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
}
