using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Models;
using System.Collections.Generic;

namespace BusinessLogicLayer.Mappers
{
    public class MapperEmployee : IMapperEmployee
    {
        public EmployeeDTO GetDTO(Employee model)
        {
            return new EmployeeDTO
            {
                Id = model.Id,
                LastName = model.LastName,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                Email = model.Email,
            };
        }
        public Employee GetNewModel(EmployeeDTO dto)
        {
            return new Employee
            {
                Id = dto.Id,
                LastName = dto.LastName,
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                Email = dto.Email,
            };
        }
        public IEnumerable<EmployeeDTO> GetDTOs(IEnumerable<Employee> models)
        {
            ICollection<EmployeeDTO> dtos = new List<EmployeeDTO>();
            foreach (var item in models) dtos.Add(GetDTO(item));
            return dtos;
        }
        public IEnumerable<Employee> GetNewModels(IEnumerable<EmployeeDTO> dtos)
        {
            ICollection<Employee> models = new List<Employee>();
            foreach (var item in dtos) models.Add(GetNewModel(item));
            return models;
        }
    }
}