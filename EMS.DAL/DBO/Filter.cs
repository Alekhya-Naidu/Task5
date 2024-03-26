using System;
using EMS.DAL.Interfaces; 

namespace EMS.DAL.DBO;

public class EmployeeFilter
{
    public string FirstName { get; set; }
    public int? LocationId { get; set; }
    public int? DepartmentId { get; set; }
    public int? EmpNo { get; set; }
}