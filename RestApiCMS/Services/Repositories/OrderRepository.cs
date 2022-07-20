using CabManagementSystem.AppContext;
using CabManagementSystem.Models;
using RestApiCMS.Services.Interfaces;

namespace RestApiCMS.Services.Repositories
{
    public class OrderRepository : IRepository<OrderModel>
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
        public bool Exist(Guid id) => orderContext.Orders.Any(x => x.UserID == id);
        public IEnumerable<OrderModel> Get() => orderContext.Orders.ToList();
        public OrderModel Get(Guid id) => orderContext.Orders.Any(x => x.ID == id) ? orderContext.Orders.First(x => x.ID == id) : new();
        public ExceptionModel Update(OrderModel item)
        {
            if (item is null)
                return ExceptionModel.VariableIsNull;
            orderContext.ChangeTracker.Clear();
            orderContext.Orders.Update(item);
            orderContext.SaveChanges();
            return ExceptionModel.Successfull;
        }
    }
}
