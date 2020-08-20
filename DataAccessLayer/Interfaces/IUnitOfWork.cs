using DataAccessLayer.Models;
using System;

namespace DataAccessLayer.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Project> Projects { get; }
        IRepository<Employee> Employees { get; }
        void Save();
    }
}