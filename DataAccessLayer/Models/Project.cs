using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CustomerName { get; set; } 
        public string ExecutingCompany { get; set; }        
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Priority { get; set; } // 0 - min
        public int? ProjectManagerId { get; set; } 
        public Employee ProjectManager { get; set; } 
        public virtual ICollection<Employee> Employees { get; set; } 
        public virtual ICollection<Employee> Executors { get; set; }

        public Project()
        {
            Employees = new List<Employee>();
            Executors = new List<Employee>();
        }
    }
}