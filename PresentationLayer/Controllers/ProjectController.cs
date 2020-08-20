using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Services;
using PresentationLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PresentationLayer.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ProjectService projectService = new ProjectService();
        private readonly EmployeeService employeeService = new EmployeeService();

        // GET: Project
        public ActionResult Index(string priorityFilter, DateTime? startDateTimeFrom, DateTime? startDateTimeTo, int priorityCurrent = 0)
        {
            var newViewModels = new ProjectDTOListViewModels();
            if (startDateTimeFrom != null && startDateTimeTo != null)
                if (startDateTimeTo < startDateTimeFrom) ModelState.AddModelError("date", "Дата 'С' меньше, чем дата 'ПО'");
            if (ModelState.IsValid) newViewModels.ApplyFilters(projectService.GetProjects(), priorityFilter, priorityCurrent, startDateTimeFrom, startDateTimeTo);
            return View(newViewModels);
        }
        // GET: Project/Details/5
        public ActionResult Details(int id = 1)
        {
            ProjectDTO project = projectService.GetProject(id);
            if (project == null) return HttpNotFound();

            var employees = employeeService.Union(project.Employees, project.Executors); 
            
            if (project.ProjectManagerId != null && (employees.Where(item => item.Id == project.ProjectManagerId)).Count() < 1)
                employees = employees.Prepend(employeeService.GetEmployee((int)project.ProjectManagerId));
            if (employees != null && employees.Count() > 0) ViewBag.Employees = employees;
            return View(project);
        }

        // GET: Project/Create
        public ActionResult Create()
        {
            var employees = employeeService.GetEmployees();
            if (employees != null && employees.Count() > 0) ViewBag.Employees = employees;
            return View(new ProjectDTO());
        }
        [HttpPost] // POST: Project/Create
        public ActionResult Create(ProjectDTO projectDTO, int[] selectedEmployees = null, int[] selectedExecutors = null)
        {
            try
            {
                if (projectDTO.EndDate < projectDTO.StartDate)
                    ModelState.AddModelError("StartDate", "Дата начала меньше даты окончания");
                if (projectDTO.Priority < 0)
                    ModelState.AddModelError("Priority", "Приоритет меньше 0");
                if (!ModelState.IsValid) throw new Exception();

                projectService.CreateProject(projectDTO, selectedEmployees, selectedExecutors);
                projectService.SaveProject();
                return RedirectToAction("Index");
            }
            catch
            {
                var employees = employeeService.GetEmployees();
                projectDTO.Employees = (IList<EmployeeDTO>)employeeService.GetEmployees(selectedEmployees, employees);
                projectDTO.Executors = (IList<EmployeeDTO>)employeeService.GetEmployees(selectedExecutors, employees);
                if (employees != null && employees.Count() > 0) ViewBag.Employees = employees;
                return View(projectDTO);
            }
        }

        // GET: Project/Edit/5
        public ActionResult Edit(int id = 1)
        {
            ProjectDTO project = projectService.GetProject(id);
            if (project == null) return HttpNotFound();
            var employees = employeeService.GetEmployees();
            if (employees != null && employees.Count() > 0) ViewBag.Employees = employees;
            return View(project);
        }
        [HttpPost] // POST: Project/Edit/5
        public ActionResult Edit(ProjectDTO projectDTO, int[] selectedEmployees = null, int[] selectedExecutors = null)
        {
            try
            {
                if (projectDTO.EndDate < projectDTO.StartDate)
                    ModelState.AddModelError("StartDate", "Дата начала меньше даты окончания");
                if (projectDTO.Priority < 0)
                    ModelState.AddModelError("Priority", "Приоритет меньше 0");
                if (!ModelState.IsValid) throw new Exception(); 

                projectService.UpdateProject(projectDTO, selectedEmployees, selectedExecutors);
                projectService.SaveProject();
                return RedirectToAction("Index");
            }
            catch
            {
                var employees = employeeService.GetEmployees();
                projectDTO.Employees = (IList<EmployeeDTO>)employeeService.GetEmployees(selectedEmployees, employees);
                projectDTO.Executors = (IList<EmployeeDTO>)employeeService.GetEmployees(selectedExecutors, employees);
                if (employees != null && employees.Count() > 0) ViewBag.Employees = employees;
                return View(projectDTO);
            }
        }

        // GET: Project/Delete/5
        public ActionResult Delete(int id)
        {
            ProjectDTO project = projectService.GetProject(id);
            if (project == null) return HttpNotFound();
            var employees = employeeService.Union(project.Employees, project.Executors); 
            // присоедениее ProjectManager
            if (project.ProjectManagerId != null && (employees.Where(item => item.Id == project.ProjectManagerId)).Count() < 1)
                employees = employees.Prepend(employeeService.GetEmployee((int)project.ProjectManagerId));
            if (employees != null && employees.Count() > 0) ViewBag.Employees = employees;
            return View(project);
        }
        [HttpPost]// POST: Project/Delete/5
        public ActionResult Delete(ProjectDTO projectDTO)
        {
            try
            {
                if (projectDTO != null)
                {
                    projectService.DeleteProject(projectDTO);
                    projectService.SaveProject();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                var employees = employeeService.Union(projectDTO.Employees, projectDTO.Executors);
                // присоедениее ProjectManager
                if (projectDTO.ProjectManagerId != null && (employees.Where(item => item.Id == projectDTO.ProjectManagerId)).Count() < 1)
                    employees = employees.Prepend(employeeService.GetEmployee((int)projectDTO.ProjectManagerId));
                if (employees != null && employees.Count() > 0) ViewBag.Employees = employees;
                return View(projectDTO);
            }
        }

        protected override void Dispose(bool disposing)
        {
            projectService.Dispose();
            base.Dispose(disposing);
        }
    }
}