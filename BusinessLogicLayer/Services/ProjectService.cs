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
    public class ProjectService : IProjectService
    {
        private readonly IMapperEmployee mapperEmployee = new MapperEmployee();
        private readonly IMapperProject mapperProject = new MapperProject();
        private readonly IEqualityComparer<ProjectDTO> projectDTOEqualityComparer = new ProjectDTOEqualityComparer();
        public IUnitOfWork DataBase { get; set; }

        public ProjectService() => DataBase = new UnitOfWork();
        public ProjectService(IUnitOfWork dataBase) => DataBase = dataBase;
        public ProjectDTO GetProject(int id)
        {
            var project = DataBase.Projects.Get(id);
            if (project != null)
            {
                var newDTO = mapperProject.GetDTO(project);
                if (project.Employees.Count > 0) newDTO.Employees = (ICollection<EmployeeDTO>)mapperEmployee.GetDTOs(project.Employees);
                if (project.Executors.Count > 0) newDTO.Executors = (ICollection<EmployeeDTO>)mapperEmployee.GetDTOs(project.Executors);
                if (project.ProjectManagerId != null && project.ProjectManager != null) newDTO.ProjectManagerId = project.ProjectManagerId;
                return newDTO;
            }
            return null;
        }
        public IEnumerable<ProjectDTO> GetProjects()
        {
            var projects = (IList<Project>)DataBase.Projects.GetAll();
            if (projects != null)
            {
                var dtos = (IList<ProjectDTO>)mapperProject.GetDTOs(projects);
                for (int i = 0; i < projects.Count; i++)
                    if (projects[i].ProjectManagerId != null && projects[i].ProjectManager != null)
                        dtos[i].ProjectManager = mapperEmployee.GetDTO(projects[i].ProjectManager);
                return dtos;
            }
            return null;
        }
        public void CreateProject(ProjectDTO dto, int[] selectedEmployees, int[] selectedExecutors)
        {
            if (dto != null)
            {
                var newProject = mapperProject.GetNewModel(dto);
                newProject = ModelBindExternalDependencies(newProject, selectedEmployees, selectedExecutors); 
                DataBase.Projects.Create(newProject);
            }
        }
        public void UpdateProject(ProjectDTO dto)
        {
            if (dto != null) DataBase.Projects.Update(mapperProject.GetNewModel(dto));
        }
        public void UpdateProject(ProjectDTO dto, int[] selectedEmployees, int[] selectedExecutors)
        {
            if (dto != null)
            {
                Project newProject = mapperProject.GetNewModel(dto);
                newProject = ModelBindExternalDependencies(newProject, selectedEmployees, selectedExecutors);
                DataBase.Projects.Update(newProject, newProject.Id, new string[] { "Employees", "Executors" });
            }
        }
        public void DeleteProject(ProjectDTO dto)
        {
            if (dto != null) DataBase.Projects.Delete(dto.Id);
        }
        public void SaveProject() => DataBase.Save();
        public IEnumerable<ProjectDTO> Union(IEnumerable<ProjectDTO> first, IEnumerable<ProjectDTO> second)
        {
            return first.Union(second, projectDTOEqualityComparer);
        }
        public void Dispose() => DataBase.Dispose();

        private Project ModelBindExternalDependencies(Project model, int[] selectedEmployees, int[] selectedExecutors)
        {
            if (selectedEmployees != null && selectedEmployees.Length > 0)
            {
                foreach (var id in selectedEmployees)
                    model.Employees.Add(DataBase.Employees.Get(id));
            }
            if (selectedExecutors != null && selectedExecutors.Length > 0)
            {
                foreach (var id in selectedExecutors)
                    model.Executors.Add(DataBase.Employees.Get(id));
            }
            if (model.ProjectManagerId != null)
            {
                var tmpProjectManager = DataBase.Employees.Get((int)model.ProjectManagerId);
                if (tmpProjectManager != null) model.ProjectManager = tmpProjectManager;
            }
            return model;
        }
    }
}