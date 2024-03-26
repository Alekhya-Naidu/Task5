using System.Collections.Generic;
using EMS.DAL.Interfaces; 

namespace EMS.DAL.DBO;

public class Role : IMasterData
{
    public int Id { get; set; }
    public int DepartmentId { get; set; }
    public string Name { get; set; }
}