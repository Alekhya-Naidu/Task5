using System;
using System.Text.Json.Serialization;

namespace EmployeeManagement;
public class EmployeeManagementSystem
{
    public enum Location
    {
        Hyderabad,
        US,
        Uk
    }
    public enum Department
    {
        PE,
        IT
    }

    public enum Role
    {
        Intern,
        Developer,
        Admin
    }
    public enum Manager
    {
        Hasnu,
        Sandeep,
        Bhagvan
    }
    public enum Project
    {
        p1,
        p2
    }
    public class Employee
    {
        public int EmpNo { get; set;}
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime Dob { get; set; }
        public string? Mail { get; set;}
        public long MobileNumber { get; set;}
        public DateTime JoiningDate { get; set;}
        public Location Location { get; set; }
        public Department Department { get; set; }
        public Role? Role { get; set; }
        public Manager? Manager { get; set; }
        public Project? Project { get; set; }
        public override string ToString()
        {
            return $"EmpNo: {EmpNo}\nName: {FirstName} {LastName}\nDob : {Dob}\nMail: {Mail}\nMobileNumber: {MobileNumber}\nJoining Date: {JoiningDate}\nLocation: {Location}\nDepartment: {Department}\nRole: {Role}\nManager: {Manager}\nProject: {Project}";
        }
    }
}

