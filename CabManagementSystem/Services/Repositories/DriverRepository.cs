using CabManagementSystem.Models;
using CabManagementSystem.Services.Interfaces;
using System.Linq.Expressions;

namespace CabManagementSystem.Services.Repositories
{
    public class DriverRepository : IRepository<DriverModel>
    {
        public ExceptionModel Create(DriverModel item)
        {
            throw new NotImplementedException();
        }

        public ExceptionModel Delete(DriverModel item)
        {
            throw new NotImplementedException();
        }

        public bool Exist(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool Exist(Expression<Func<DriverModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DriverModel> Get()
        {
            throw new NotImplementedException();
        }

        public DriverModel Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public DriverModel Get(Expression<Func<DriverModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public ExceptionModel Update(DriverModel item)
        {
            throw new NotImplementedException();
        }
    }
}
