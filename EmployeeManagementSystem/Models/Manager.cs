using System.Collections.Generic;

namespace EmployeeManagement;

public class Manager : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
}