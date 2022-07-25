using CabManagementSystem.Models;
using System.Linq.Expressions;

namespace CabManagementSystem.Services.Interfaces
{
    public interface IRepository<T>
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
    public interface IUserRepository<T> : IRepository<T>
    {
        string HashPassword(string password);
    }
    public interface IOrderRepository<T> : IRepository<T>
    {
        bool AlreadyOrder(Guid id);
    }
    public interface IDriverRepository<T>
    {
        IEnumerable<T> Get();
        T Get(Guid id);
        T Get(Expression<Func<T, bool>> predicate);
    }
    public interface ITaxiRepository<T> : IRepository<T>
    {
        void ChangeTracker();
    }
}
