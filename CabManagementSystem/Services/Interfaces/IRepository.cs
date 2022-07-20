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
        bool Exist(Expression<Func<T, bool>> predicate);
    }
    public interface IUserRepository<T> : IRepository<T> where T : class
    {
        string HashPassword(string password);
    }
    public interface IOrderRepository<T> : IRepository<T> where T : class
    {
        bool AlreadyOrder(Guid id);
    }
    public interface IDriverRepository<T> where T : class
    {
        IEnumerable<T> Get();
        T Get(Guid id);
        T Get(Expression<Func<T, bool>> predicate);
    }
    public interface ITaxiRepository<T> : IRepository<T> where T : class
    {
        void ChangeTracker();
    }
}
