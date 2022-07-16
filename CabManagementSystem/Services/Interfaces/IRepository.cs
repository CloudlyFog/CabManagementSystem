using CabManagementSystem.Models;
using System.Linq.Expressions;

namespace CabManagementSystem.Services.Interfaces
{
    public interface IRepository<T> where T : class
    {
        ExceptionModel Create(T item);
        IEnumerable<T> Get();
        T Get(Guid id);
        T Get(Expression<Func<T, bool>> predicate);
        ExceptionModel Update(T item);
        ExceptionModel Delete(T item);
        bool Exist(Guid id);
    }
}
