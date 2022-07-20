using CabManagementSystem.Models;

namespace RestApiCMS.Services.Interfaces
{
    public interface IRepository<T> where T : class
    {
        ExceptionModel Create(T item);
        IEnumerable<T> Get();
        T Get(Guid id);
        ExceptionModel Update(T item);
        ExceptionModel Delete(T item);
        bool Exist(Guid id);
    }
}
