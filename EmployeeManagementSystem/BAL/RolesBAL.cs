using System.Collections.Generic;

namespace EmployeeManagement;

public class RolesBAL : IRolesBAL
{
    private readonly RolesDAL _rolesDAL;

    public RolesBAL(RolesDAL rolesDAL)
    {
        _rolesDAL = rolesDAL;
    }

    public List<Role> GetAllRoles()
    {
        return _rolesDAL.GetAllRoles();
    }

    public Role GetRoleById(int roleId)
    {
        return _rolesDAL.GetRoleById(roleId);
    }

    public bool AddRole(int departmentId, int roleId, string roleName)
    {
        return _rolesDAL.AddRole(departmentId, roleId, roleName);
    }

    public bool UpdateRolesFile(List<Role> roles)
    {
        return _rolesDAL.UpdateRolesFile(roles);
    }
    
    public List<Department> GetAllDepartments()
    {
        return _rolesDAL.GetAllDepartments();
    }
}
