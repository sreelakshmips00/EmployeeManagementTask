using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeTask.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Department { get; set; }
        public List<Dependent> Dependents { get; set; }
    }

    public class Dependent
    {
        public int DependentID { get; set; }
        public int EmployeeID { get; set; }
        public string DependentName { get; set; }
        public string Relationship { get; set; }
        public int Age { get; set; }
    }
}