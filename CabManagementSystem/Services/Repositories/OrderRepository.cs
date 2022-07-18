using CabManagementSystem.AppContext;
using CabManagementSystem.Models;
using CabManagementSystem.Services.Interfaces;
using System.Linq.Expressions;

namespace CabManagementSystem.Services.Repositories
{
    public class OrderRepository : OrderContext, IOrderRepository<OrderModel>, IDriverRepository<DriverModel>
    {
        private readonly ApplicationContext applicationContext;
        private readonly OrderContext orderContext;
        private readonly BankSystem.AppContext.BankAccountContext bankAccountContext;
        private readonly BankSystem.AppContext.BankContext bankContext;
        public OrderRepository()
        {
            applicationContext = new();
            orderContext = new();
            bankAccountContext = new();
            bankContext = new();
        }
        public OrderRepository(string queryConnectionBank)
        {
            applicationContext = new();
            orderContext = new();
            bankAccountContext = new(queryConnectionBank);
            bankContext = new(queryConnectionBank);
        }

        /// <summary>
        /// adds data of user order and withdraw money from account
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public ExceptionModel Create(OrderModel item)
        {
            if (item is null)
                return ExceptionModel.VariableIsNull;
            var driver = orderContext.Drivers.FirstOrDefault(x => !x.Busy && x.TaxiPrice == item.Price);
            if (driver is null)
                return ExceptionModel.VariableIsNull;
            var taxi = orderContext.Taxi.FirstOrDefault(x => x.ID == driver.TaxiID);
            if (taxi is null)
                return ExceptionModel.VariableIsNull;
            driver.Busy = true;
            taxi.Busy = true;
            orderContext.Drivers.Update(driver);
            orderContext.Taxi.Update(taxi);
            item.DriverName = driver.Name;
            item.TaxiID = taxi.ID;
            orderContext.Orders.Add(item);
            applicationContext.Users.FirstOrDefault(x => x.ID == item.UserID).HasOrder = true; // sets that definite user ordered taxi
            if (bankAccountContext.Withdraw(bankContext.BankAccounts.FirstOrDefault(x => x.UserBankAccountID == item.UserID), item.Price.GetHashCode()) != BankSystem.Models.ExceptionModel.Successfull)
                return ExceptionModel.OperationFailed;
            bankAccountContext.SaveChanges();
            orderContext.SaveChanges();
            return ExceptionModel.Successfull;
        }

        /// <summary>
        /// removes data of user order and accrual money on account
        /// </summary>
        /// <param name="order"></param>
        public ExceptionModel Delete(OrderModel item)
        {
            if (item is null)
                return ExceptionModel.VariableIsNull;
            var driver = orderContext.Drivers.FirstOrDefault(x => x.TaxiID == item.TaxiID);
            var taxi = orderContext.Taxi.FirstOrDefault(x => x.ID == driver.TaxiID);
            driver.Busy = false;
            taxi.Busy = false;
            orderContext.Drivers.Update(driver);
            orderContext.Taxi.Update(taxi);
            orderContext.Orders.Remove(item);
            applicationContext.Users.FirstOrDefault(x => x.ID == item.UserID).HasOrder = false;
            if (bankAccountContext.Accrual(bankContext.BankAccounts.FirstOrDefault(x => x.UserBankAccountID == item.UserID), item.Price.GetHashCode()) != BankSystem.Models.ExceptionModel.Successfull)
                return ExceptionModel.OperationFailed;
            bankAccountContext.SaveChanges();
            orderContext.SaveChanges();
            return ExceptionModel.Successfull;
        }

        /// <summary>
        /// checks on existing in the database definite order
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Exist(Guid id) => orderContext.Orders.Any(x => x.UserID == id);

        /// <summary>
        /// gets sequence of orders from database
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OrderModel> Get() => orderContext.Orders.ToList();

        /// <summary>
        /// gets order with definite id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OrderModel Get(Guid id) => orderContext.Orders.Any(x => x.ID == id) ? orderContext.Orders.First(x => x.ID == id) : new();

        /// <summary>
        /// gets order with user condition
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public OrderModel? Get(Expression<Func<OrderModel, bool>> predicate) => orderContext.Orders.FirstOrDefault(predicate);

        /// <summary>
        /// updates data of user order
        /// </summary>
        /// <param name="order"></param>
        public ExceptionModel Update(OrderModel item)
        {
            if (item is null)
                return ExceptionModel.VariableIsNull;
            orderContext.ChangeTracker.Clear();
            orderContext.Orders.Update(item);
            orderContext.SaveChanges();
            return ExceptionModel.Successfull;
        }

        /// <summary>
        /// gets driver with user condition
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public DriverModel? Get(Expression<Func<DriverModel, bool>> predicate) => orderContext.Drivers.FirstOrDefault(predicate);

        /// <summary>
        /// gets sequence of drivers from the database
        /// </summary>
        /// <returns></returns>
        IEnumerable<DriverModel> IDriverRepository<DriverModel>.Get() => orderContext.Drivers.ToList();

        /// <summary>
        /// gets driver with definite id from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DriverModel IDriverRepository<DriverModel>.Get(Guid id) => orderContext.Drivers.Any(x => x.DriverID == id) ? orderContext.Drivers.First(x => x.DriverID == id) : new();
    }
}
