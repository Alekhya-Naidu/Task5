using System.Collections.Generic;

namespace EmployeeManagement;

public interface IRolesBAL
{
    List<Role> GetAllRoles();
    Role GetRoleById(int roleId);
    Role GetRoleFromName(string roleInput);
    bool AddRole(int departmentId, int roleId, string roleName);
    bool UpdateRoles(List<Role> roles);
    List<Department> GetAllDepartments();
}