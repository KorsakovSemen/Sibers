using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Services;
using System.Linq;
using System.Web.Mvc;

namespace PresentationLayer.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ProjectService projectService = new ProjectService();
        private readonly EmployeeService employeeService = new EmployeeService();

        // GET: Employee
        public ActionResult Index()
        {
            return View(employeeService.GetEmployees());
        }
        // GET: Employee/Details/5
        public ActionResult Details(int id = 1)
        {
            EmployeeDTO employee = employeeService.GetEmployee(id);
            if (employee == null) return HttpNotFound();
            var projectDTOs = projectService.Union(employee.EmployeeInProjects, employee.ExecutorInProjects);
            projectDTOs = projectService.Union(projectDTOs, employeeService.ManagerProjects(employee.Id, projectService.GetProjects()));
            if (projectDTOs != null && projectDTOs.Count() > 0) ViewBag.Projects = projectDTOs;
            return View(employee);
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: Employee/Create
        [HttpPost]
        public ActionResult Create(EmployeeDTO employeeDTO)
        {
            try
            {
                employeeService.CreateEmployee(employeeDTO);
                employeeService.SaveEmployee();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Employee/Edit/5
        public ActionResult Edit(int id = 1)
        {
            EmployeeDTO employee = employeeService.GetEmployee(id);
            if (employee == null) return HttpNotFound();
            return View(employee);
        }
        // POST: Employee/Edit/5
        [HttpPost]
        public ActionResult Edit(EmployeeDTO employeeDTO)
        {
            try
            {
                if (employeeDTO != null)
                {
                    employeeService.UpdateEmployee(employeeDTO);
                    employeeService.SaveEmployee();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Employee/Delete/5
        public ActionResult Delete(int id)
        {
            EmployeeDTO employee = employeeService.GetEmployee(id);
            if (employee == null) return HttpNotFound();
            var projectDTOs = projectService.Union(employee.EmployeeInProjects, employee.ExecutorInProjects);
            projectDTOs = projectService.Union(projectDTOs, employeeService.ManagerProjects(employee.Id, projectService.GetProjects()));
            if (projectDTOs != null && projectDTOs.Count() > 0) ViewBag.Projects = projectDTOs;
            return View(employee);
        }
        // POST: Employee/Delete/5
        [HttpPost]
        public ActionResult Delete(EmployeeDTO employeeDTO)
        {
            try
            {
                if (employeeDTO != null)
                {
                    employeeService.DeleteEmployee(employeeDTO);
                    employeeService.SaveEmployee();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return Redirect("/Employee/Delete/" + employeeDTO.Id);
            }
        }

        protected override void Dispose(bool disposing)
        {
            employeeService.Dispose();
            base.Dispose(disposing);
        }
    }
}