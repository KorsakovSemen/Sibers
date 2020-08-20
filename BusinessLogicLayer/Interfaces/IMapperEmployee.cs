using BusinessLogicLayer.DTO;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Interfaces
{
    public interface IMapperEmployee : IMapper<Employee, EmployeeDTO>
    {
    }
}