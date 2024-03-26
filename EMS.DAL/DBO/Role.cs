using System.Collections.Generic;

namespace EmployeeManagement;

public class Role : IMasterData
{
    public int Id { get; set; }
    public int DepartmentId { get; set; }
    public string Name { get; set; }
}