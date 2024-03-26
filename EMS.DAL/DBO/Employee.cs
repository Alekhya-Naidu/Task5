using System;
using System.Text;
using System.Text.Json.Serialization;

namespace EMS.DAL.DBO
{
    public class Employee
    {
        public int EmpNo { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime Dob { get; set; }
        public string? Mail { get; set; }
        public string MobileNumber { get; set; }
        public DateTime JoiningDate { get; set; }
        public int LocationId { get; set; }
        public int DepartmentId { get; set; }
        public int RoleId { get; set; }
        public int ManagerId { get; set; }
        public int ProjectId { get; set; }

        [JsonIgnore]
        public Location Location { get; set; }
        [JsonIgnore]
        public Department Department { get; set; }
        [JsonIgnore]
        public Role Role { get; set; }
        [JsonIgnore]
        public Manager Manager { get; set; }
        [JsonIgnore]
        public Project Project { get; set; }
    }
}
