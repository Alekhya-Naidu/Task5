using System.Collections.Generic;

namespace EmployeeManagement;

public interface IRolesDAL
{
    List<Role> GetAllRoles();
    Role GetRoleFromName(string roleInput);
    Role GetRoleById(int roleId);
    int GetRoleId(Role role);
    bool AddRole(int departmentId, int roleId, string roleName);
    bool UpdateRoles(List<Role> roles);
    List<Department> GetAllDepartments();
}
