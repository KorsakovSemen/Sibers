using BusinessLogicLayer.DTO;
using System.Collections.Generic;

namespace BusinessLogicLayer.Infrastructure.Comparers
{
    public class EmployeeDTOEqualityComparer : IEqualityComparer<EmployeeDTO>
    {
        public bool Equals(EmployeeDTO x, EmployeeDTO y)
        {
            return x.Id == y.Id;
        }
        public int GetHashCode(EmployeeDTO obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}