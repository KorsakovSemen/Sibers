using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.DTO
{
    public class EmployeeDTO
    {
        public int Id { get; set; }
        [Display(Name = "Фамилия")] public string LastName { get; set; }
        [Display(Name = "Имя")] public string FirstName { get; set; }
        [Display(Name = "Отчество")] public string MiddleName { get; set; }
        [EmailAddress(ErrorMessage = "Некорректный e-mail"), Display(Name = "E-mail")] public string Email { get; set; }
        [Display(Name = "Сотрудник в ")] public ICollection<ProjectDTO> EmployeeInProjects { get; set; } 
        [Display(Name = "Исполнитель в ")] public ICollection<ProjectDTO> ExecutorInProjects { get; set; }

        public EmployeeDTO()
        {
            EmployeeInProjects = new List<ProjectDTO>();
            ExecutorInProjects = new List<ProjectDTO>();
        }
    }
}