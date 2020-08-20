using BusinessLogicLayer.DTO;
using System.Collections.Generic;

namespace BusinessLogicLayer.Interfaces
{
    public interface IProjectService
    {
        ProjectDTO GetProject(int id);
        IEnumerable<ProjectDTO> GetProjects();
        void CreateProject(ProjectDTO dto, int[] selectedEmployees = null, int[] selectedExecutors = null);
        void UpdateProject(ProjectDTO dto);
        void UpdateProject(ProjectDTO dto, int[] selectedEmployees, int[] selectedExecutors);
        void DeleteProject(ProjectDTO dto);
        void SaveProject();
        IEnumerable<ProjectDTO> Union(IEnumerable<ProjectDTO> first, IEnumerable<ProjectDTO> second);
        void Dispose();
    }
}