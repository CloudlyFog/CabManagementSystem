using CabManagementSystem.AppContext;
using CabManagementSystem.Models;
using CabManagementSystem.Services.Interfaces;
using System.Linq.Expressions;
using BankAccountModel = BankSystem.Models.BankAccountModel;

namespace CabManagementSystem.Services.Repositories
{
    public class OrderRepository : OrderContext, IOrderRepository<OrderModel>, IDriverRepository<DriverModel>
    {
        private readonly OrderContext orderContext;
        private readonly BankSystem.AppContext.BankContext bankContext;
        private readonly IUserRepository<UserModel> userRepository;
        private readonly ITaxiRepository<TaxiModel> taxiRepository;
        private readonly IBankAccountRepository<BankAccountModel> bankAccountRepository;
        private readonly IDriverRepository<DriverModel> orderRepository;
        public OrderRepository()
        {
            orderContext = new();
            bankContext = new();
            userRepository = new UserRepository();
            bankAccountRepository = new BankAccountRepository();
            taxiRepository = new TaxiRepository();
            orderRepository = new OrderRepository();
        }
        public OrderRepository(string queryConnectionBank)
        {
            orderContext = new();
            bankContext = new(queryConnectionBank);
            userRepository = new UserRepository();
            bankAccountRepository = new BankAccountRepository();
            taxiRepository = new TaxiRepository();
            orderRepository = new OrderRepository();
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
            var driver = orderRepository.Get(x => !x.Busy && x.TaxiPrice == item.Price);
            if (driver is null)
                return ExceptionModel.VariableIsNull;
            var taxi = taxiRepository.Get(x => x.ID == driver.TaxiID);
            if (taxi is null)
                return ExceptionModel.VariableIsNull;
            driver.Busy = true;
            taxi.Busy = true;
            Drivers.Update(driver);
            Taxi.Update(taxi);
            item.DriverName = driver.Name;
            item.TaxiID = taxi.ID;
            Orders.Add(item);
            userRepository.Get(x => x.ID == item.UserID).HasOrder = false; // sets that definite user ordered taxi
            if (bankAccountRepository.Withdraw(bankContext.BankAccounts.FirstOrDefault(x => x.UserBankAccountID == item.UserID), item.Price.GetHashCode()) != BankSystem.Models.ExceptionModel.Successfull)
                return ExceptionModel.OperationFailed;
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
            var driver = orderRepository.Get(x => !x.Busy && x.TaxiPrice == item.Price);
            if (driver is null)
                return ExceptionModel.VariableIsNull;
            var taxi = taxiRepository.Get(x => x.ID == driver.TaxiID);
            driver.Busy = false;
            taxi.Busy = false;
            Drivers.Update(driver);
            Taxi.Update(taxi);
            Orders.Remove(item);
            userRepository.Get(x => x.ID == item.UserID).HasOrder = false; // sets that definite user ordered taxi
            if (bankAccountRepository.Accrual(bankContext.BankAccounts.FirstOrDefault(x => x.UserBankAccountID == item.UserID), item.Price.GetHashCode()) != BankSystem.Models.ExceptionModel.Successfull)
                return ExceptionModel.OperationFailed;
            SaveChanges();
            return ExceptionModel.Successfull;
        }

        /// <summary>
        /// checks on existing in the database definite order
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Exist(Guid id) => Orders.Any(x => x.UserID == id);

        /// <summary>
        /// gets sequence of orders from database
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OrderModel> Get() => Orders.ToList();

        /// <summary>
        /// gets order with definite id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OrderModel Get(Guid id) => Orders.Any(x => x.ID == id) ? Orders.First(x => x.ID == id) : new();

        /// <summary>
        /// gets order with user condition
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public OrderModel? Get(Expression<Func<OrderModel, bool>> predicate) => Orders.FirstOrDefault(predicate);

        /// <summary>
        /// updates data of user order
        /// </summary>
        /// <param name="order"></param>
        public ExceptionModel Update(OrderModel item)
        {
            if (item is null)
                return ExceptionModel.VariableIsNull;
            ChangeTracker.Clear();
            Orders.Update(item);
            SaveChanges();
            return ExceptionModel.Successfull;
        }

        /// <summary>
        /// gets driver with user condition
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public DriverModel? Get(Expression<Func<DriverModel, bool>> predicate) => Drivers.FirstOrDefault(predicate);

        /// <summary>
        /// gets sequence of drivers from the database
        /// </summary>
        /// <returns></returns>
        IEnumerable<DriverModel> IDriverRepository<DriverModel>.Get() => Drivers.ToList();

        /// <summary>
        /// gets driver with definite id from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DriverModel IDriverRepository<DriverModel>.Get(Guid id) => Drivers.Any(x => x.DriverID == id) ? Drivers.First(x => x.DriverID == id) : new();

        public bool Exist(Expression<Func<OrderModel, bool>> predicate) => Orders.Any(predicate);
    }
}
