using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Infrastructure.Comparers;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Mappers;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using DataAccessLayer.Repository;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IMapperEmployee mapperEmployee = new MapperEmployee();
        private readonly IMapperProject mapperProject = new MapperProject();
        private readonly IEqualityComparer<EmployeeDTO> employeeDTOEqualityComparer = new EmployeeDTOEqualityComparer();
        public IUnitOfWork DataBase { get; set; }

        public EmployeeService() => DataBase = new UnitOfWork();
        public EmployeeService(IUnitOfWork dataBase) => DataBase = dataBase;
        public EmployeeDTO GetEmployee(int id)
        {
            var employee = DataBase.Employees.Get(id);
            if (employee != null)
            {
                var newDTO = mapperEmployee.GetDTO(employee);
                if (employee.EmployeeInProjects.Count > 0) newDTO.EmployeeInProjects = (ICollection<ProjectDTO>)mapperProject.GetDTOs(employee.EmployeeInProjects);
                if (employee.ExecutorInProjects.Count > 0) newDTO.ExecutorInProjects = (ICollection<ProjectDTO>)mapperProject.GetDTOs(employee.ExecutorInProjects);
                return newDTO;
            }
            return null;
        }
        public IEnumerable<EmployeeDTO> GetEmployees()
        {
            var employees = DataBase.Employees.GetAll();
            if (employees != null) return mapperEmployee.GetDTOs(employees);
            return null;
        }
        public IEnumerable<EmployeeDTO> GetEmployees(int[] idEmployees, IEnumerable<EmployeeDTO> employeesDTOs)
        {
            IList<EmployeeDTO> tmpEmployees = new List<EmployeeDTO>();
            if (idEmployees != null && idEmployees.Length > 0 && employeesDTOs != null && employeesDTOs.Count() > 0)
            {
                foreach (var employee in employeesDTOs)
                    foreach (var id in idEmployees)
                    {
                        if (employee.Id == id) tmpEmployees.Add(employee);
                    }
            }
            return tmpEmployees;
        }
        public void CreateEmployee(EmployeeDTO dto)
        {
            if (dto != null)
            {
                var newEmployee = mapperEmployee.GetNewModel(dto);
                if (dto.EmployeeInProjects.Count > 0) newEmployee.EmployeeInProjects = (ICollection<Project>)mapperProject.GetNewModels(dto.EmployeeInProjects);
                if (dto.ExecutorInProjects.Count > 0) newEmployee.ExecutorInProjects = (ICollection<Project>)mapperProject.GetNewModels(dto.ExecutorInProjects);
                DataBase.Employees.Create(newEmployee);
            }
        }        
        public void UpdateEmployee(EmployeeDTO dto)
        {
            if (dto != null) DataBase.Employees.Update(mapperEmployee.GetNewModel(dto));
        }
        public void DeleteEmployee(EmployeeDTO dto)
        {
            if (dto != null) DataBase.Employees.Delete(dto.Id);
        }
        public void SaveEmployee() => DataBase.Save();
        public IEnumerable<EmployeeDTO> Union(IEnumerable<EmployeeDTO> first, IEnumerable<EmployeeDTO> second)
        {
            return first.Union(second, employeeDTOEqualityComparer);
        }
        public IEnumerable<ProjectDTO> ManagerProjects(int managerId, IEnumerable<ProjectDTO> projectDTOs)
        {
            IEnumerable<ProjectDTO> managerProjects = new List<ProjectDTO>();
            if (projectDTOs != null && projectDTOs.Count() > 0)
                managerProjects = projectDTOs.Where(item => item.ProjectManagerId == managerId);
            return managerProjects;
        }
        public void Dispose() => DataBase.Dispose();
    }
}