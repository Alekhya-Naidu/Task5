using System.Collections.Generic;
using EMS.BAL.Interfaces;
using EMS.DAL.Interfaces;
using EMS.DAL.DBO;

namespace EMS.BAL.BAL;

public class RolesBAL : IRolesBAL
{
    private readonly IRolesDAL _rolesDAL;

    public RolesBAL(IRolesDAL rolesDAL)
    {
        _rolesDAL = rolesDAL;
    }

    public List<Role> GetAllRoles<Role>(string rolefilePath)
    {
        return _rolesDAL.GetAllRoles<Role>(rolefilePath);
    }

    public Role GetRoleFromName(string roleInput)
    {
       return _rolesDAL.GetRoleFromName(roleInput);
    }
    public Role GetRoleById(int roleId)
    {
        return _rolesDAL.GetRoleById(roleId);
    }

    public bool AddRole(int departmentId, int roleId, string roleName)
    {
        return _rolesDAL.AddRole(departmentId, roleId, roleName);
    }

    public bool UpdateRoles(List<Role> roles)
    {
        return _rolesDAL.UpdateRoles(roles);
    }
}
