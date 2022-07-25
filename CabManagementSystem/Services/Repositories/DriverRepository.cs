using CabManagementSystem.AppContext;
using CabManagementSystem.Models;
using CabManagementSystem.Services.Interfaces;
using System.Linq.Expressions;

namespace CabManagementSystem.Services.Repositories
{
    public class DriverRepository : IDriverRepository<DriverModel>
    {
        private readonly OrderContext orderContext = new();

        /// <summary>
        /// gets driver with user condition
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public DriverModel? Get(Expression<Func<DriverModel, bool>> predicate) => orderContext.Drivers.Any(predicate) ? orderContext.Drivers.First(predicate) : new();

        /// <summary>
        /// gets sequence of drivers from the database
        /// </summary>
        /// <returns></returns>
        IEnumerable<DriverModel> IDriverRepository<DriverModel>.Get() => orderContext.Drivers;

        /// <summary>
        /// gets driver with definite id from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DriverModel IDriverRepository<DriverModel>.Get(Guid id) => orderContext.Drivers.Any(x => x.DriverID == id) ? orderContext.Drivers.First(x => x.DriverID == id) : new();
    }
}
