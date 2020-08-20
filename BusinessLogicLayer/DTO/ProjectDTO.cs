using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.DTO
{
    public class ProjectDTO
    {
        public int Id { get; set; }
        [Display(Name = "Название")] public string Name { get; set; }
        [Display(Name = "Компания-заказчик")] public string CustomerName { get; set; } 
        [Display(Name = "Компания-исполнитель")] public string ExecutingCompany { get; set; }    

        [DataType(DataType.Date), Required, Display(Name = "Дата начала")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date), Required, Display(Name = "Дата окончания")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Приоритет")] public int Priority { get; set; } // 0 - min
        [Display(Name = "Руководитель проекта")] public int? ProjectManagerId { get; set; } 
        [Display(Name = "Руководитель проекта")] public EmployeeDTO ProjectManager { get; set; }
        [Display(Name = "Сотрудники")] public virtual ICollection<EmployeeDTO> Employees { get; set; } 
        [Display(Name = "Исполнители")] public virtual ICollection<EmployeeDTO> Executors { get; set; } 
       
        public int EmployeesAmount { get; set; }
        
        public int ExecutorsAmount { get; set; }

        public ProjectDTO()
        {
            Employees = new List<EmployeeDTO>();
            Executors = new List<EmployeeDTO>();
            ProjectManagerId = null;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddYears(1);
        }
    }
}