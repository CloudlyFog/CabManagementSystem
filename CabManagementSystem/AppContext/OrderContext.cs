using CabManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CabManagementSystem.AppContext
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options) => Database.EnsureCreated();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseSqlServer(
                @"Server=localhost\\SQLEXPRESS;Data Source=maxim;Initial Catalog=CabManagementSystem;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False");
        }

        private readonly BankAccountContext bankAccountContext = new(new DbContextOptions<BankAccountContext>());
        private readonly BankContext bankContext = new(new DbContextOptions<BankContext>());
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<DriverModel> Drivers { get; set; }
        public DbSet<TaxiModel> Taxi { get; set; }

        /// <summary>
        /// adds data of user order and withdraw money from account
        /// </summary>
        /// <param name="order"></param>
        public ExceptionModel CreateOrder(OrderModel order)
        {
            if (order is null)
                return ExceptionModel.VariableIsNull;
            var driver = Drivers.FirstOrDefault(x => !x.Busy && x.TaxiPrice == order.Price);
            if (driver is null)
                return ExceptionModel.VariableIsNull;
            var taxi = Taxi.FirstOrDefault(x => x.ID == driver.TaxiID);
            if (taxi is null)
                return ExceptionModel.VariableIsNull;
            driver.Busy = true;
            taxi.Busy = true;
            Drivers.Update(driver);
            Taxi.Update(taxi);
            order.DriverName = driver.Name;
            order.TaxiID = taxi.ID;
            Orders.Add(order);
            bankAccountContext.Users.FirstOrDefault(x => x.ID == order.UserID).HasOrder = true; // sets that definite user ordered taxi
            if (bankAccountContext.Withdraw(bankContext.BankAccounts.FirstOrDefault(x => x.UserBankAccountID == order.UserID), order.Price.GetHashCode()) != ExceptionModel.Successfull)
                return ExceptionModel.OperationFailed;
            bankAccountContext.SaveChanges();
            SaveChanges();
            return ExceptionModel.Successfull;
        }

        /// <summary>
        /// updates data of user order
        /// </summary>
        /// <param name="order"></param>
        public ExceptionModel UpdateOrder(OrderModel order)
        {
            if (order is null)
                return ExceptionModel.VariableIsNull;
            ChangeTracker.Clear();
            Orders.Update(order);
            SaveChanges();
            return ExceptionModel.Successfull;
        }

        /// <summary>
        /// removes data of user order and accrual money on account
        /// </summary>
        /// <param name="order"></param>
        public ExceptionModel DeleteOrder(OrderModel order)
        {
            if (order is null)
                return ExceptionModel.VariableIsNull;
            var driver = Drivers.FirstOrDefault(x => x.TaxiID == order.TaxiID);
            var taxi = Taxi.FirstOrDefault(x => x.ID == driver.TaxiID);
            driver.Busy = false;
            taxi.Busy = false;
            Drivers.Update(driver);
            Taxi.Update(taxi);
            Orders.Remove(order);
            bankAccountContext.Users.FirstOrDefault(x => x.ID == order.UserID).HasOrder = false;
            if (bankAccountContext.Accrual(bankContext.BankAccounts.FirstOrDefault(x => x.UserBankAccountID == order.UserID), order.Price.GetHashCode()) != ExceptionModel.Successfull)
                return ExceptionModel.OperationFailed;
            bankAccountContext.SaveChanges();
            SaveChanges();
            return ExceptionModel.Successfull;
        }

        /// <summary>
        /// defines user already has order or not
        /// </summary>
        /// <param name="userID"></param>
        /// <returns><see langword="true"/> if the database there's order with the same user id</returns>
        public bool AlreadyOrder(Guid userID) => Orders.Any(x => x.UserID == userID);
    }
}
