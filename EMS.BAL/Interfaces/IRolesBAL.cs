using System.Collections.Generic;
using EMS.DAL.DBO;

namespace EMS.BAL.Interfaces;

public interface IRolesBAL
{
    List<Role> GetAllRoles<Role>(string rolefilePath);
    Role GetRoleFromName(string roleInput);
    Role GetRoleById(int roleId);
    bool AddRole(int departmentId, int roleId, string roleName);
    bool UpdateRoles(List<Role> roles);
}