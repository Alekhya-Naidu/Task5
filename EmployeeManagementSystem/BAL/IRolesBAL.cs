using System.Collections.Generic;

namespace EmployeeManagement;

public interface IRolesBAL
{
    List<Role> GetAllRoles();
    Role GetRoleById(int roleId);
    bool UpdateRolesFile(List<Role> roles);
    List<Department> GetAllDepartments();
}