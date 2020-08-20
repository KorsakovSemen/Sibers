using BusinessLogicLayer.DTO;
using System.Collections.Generic;

namespace BusinessLogicLayer.Infrastructure.Comparers
{
    public class ProjectDTOEqualityComparer : IEqualityComparer<ProjectDTO>
    {
        public bool Equals(ProjectDTO x, ProjectDTO y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(ProjectDTO obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}