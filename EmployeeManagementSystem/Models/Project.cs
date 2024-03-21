using System.Collections.Generic;

namespace EmployeeManagement;

public class Project : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
}