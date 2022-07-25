using CabManagementSystem.AppContext;
using CabManagementSystem.Models;
using CabManagementSystem.Services.Interfaces;
using System.Linq.Expressions;
using BankAccountModel = BankSystem.Models.BankAccountModel;

namespace CabManagementSystem.Services.Repositories
{
    public class OrderRepository : OrderContext, IOrderRepository<OrderModel>
    {
        private readonly IUserRepository<UserModel> userRepository;
        private readonly ITaxiRepository<TaxiModel> taxiRepository;
        private readonly BankSystem.Services.Interfaces.IBankAccountRepository<BankAccountModel> bankAccountRepository;
        private readonly IDriverRepository<DriverModel> driverRepository;
        public OrderRepository()
        {
            userRepository = new UserRepository();
            bankAccountRepository = new BankSystem.Services.Repositories.BankAccountRepository();
            taxiRepository = new TaxiRepository();
            driverRepository = new DriverRepository();
        }
        public OrderRepository(string queryConnectionBank)
        {
            userRepository = new UserRepository();
            bankAccountRepository = new BankSystem.Services.Repositories.BankAccountRepository(queryConnectionBank);
            driverRepository = new DriverRepository();
            taxiRepository = new TaxiRepository();
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
            var driver = driverRepository.Get(x => !x.Busy && x.TaxiPrice == item.Price);
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
            var user = userRepository.Get(x => x.ID == item.UserID);  // sets that definite user ordered taxi
            user.HasOrder = true;
            userRepository.Update(user);
            if (bankAccountRepository.Withdraw(bankAccountRepository.Get(x => x.UserBankAccountID == item.UserID), item.Price.GetHashCode()) != BankSystem.Models.ExceptionModel.Successfull)
                return ExceptionModel.OperationFailed;
            SaveChanges();
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
            var driver = driverRepository.Get(x => x.Name == item.DriverName);
            if (driver is null)
                return ExceptionModel.VariableIsNull;
            var taxi = taxiRepository.Get(x => x.ID == item.TaxiID);
            driver.Busy = false;
            taxi.Busy = false;
            Drivers.Update(driver);
            Taxi.Update(taxi);
            Orders.Remove(item);
            var user = userRepository.Get(x => x.ID == item.UserID); // sets that definite user ordered taxi
            user.HasOrder = false;
            userRepository.Update(user);
            if (bankAccountRepository.Accrual(bankAccountRepository.Get(x => x.UserBankAccountID == item.UserID), item.Price.GetHashCode()) != BankSystem.Models.ExceptionModel.Successfull)
                return ExceptionModel.OperationFailed;
            SaveChanges();
            return ExceptionModel.Successfull;
        }

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
        public OrderModel? Get(Expression<Func<OrderModel, bool>> predicate) => Orders.Any(predicate) ? Orders.First(predicate) : new();

        /// <summary>
        /// checks on existing in the database definite order
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Exist(Guid id) => Orders.Any(x => x.UserID == id);

        /// <summary>
        /// checks on existing in the database definite order by func condition
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool Exist(Expression<Func<OrderModel, bool>> predicate) => Orders.Any(predicate);

        bool IOrderRepository<OrderModel>.AlreadyOrder(Guid id) => AlreadyOrder(id);
    }
}
