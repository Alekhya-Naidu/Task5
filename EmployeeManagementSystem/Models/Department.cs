using System.Collections.Generic;

namespace EmployeeManagement;

public class Department : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
}
