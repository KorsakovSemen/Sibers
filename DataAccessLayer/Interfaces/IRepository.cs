using System;
using System.Collections.Generic;

namespace DataAccessLayer.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T Get(int id);
        IEnumerable<T> GetAll();

        void Create(T item);        
        void Update(T item);      
        void Update(T entity, int id, string[] navigationProperties = null);
        void Delete(T entity);
        void Delete(int id);

        IEnumerable<T> Find(Func<T, Boolean> predicate);
    }
}