using System.Collections.Generic;
using EMS.DAL.Interfaces; 

namespace EMS.DAL.DBO;

public class Department : IMasterData
{
    public int Id { get; set; }
    public string Name { get; set; }
}
