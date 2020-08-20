using DataAccessLayer.Context;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using System;

namespace DataAccessLayer.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProjectDataContext dataContext;
        private ProjectRepository projectRepository;
        private EmployeeRepository employeeRepository;
        private bool disposed = false;

        public UnitOfWork() => dataContext = new ProjectDataContext();

        public IRepository<Project> Projects
        {
            get
            {
                if (projectRepository == null) projectRepository = new ProjectRepository(dataContext);
                return projectRepository;
            }
        }
        public IRepository<Employee> Employees
        {
            get
            {
                if (employeeRepository == null) employeeRepository = new EmployeeRepository(dataContext);
                return employeeRepository;
            }
        }
        public void Save() => dataContext.SaveChanges();
        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing) dataContext.Dispose();
                this.disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
