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
        public void CreateOrder(OrderModel order)
        {
            if (order is null)
                throw new ArgumentNullException();
            var driver = Drivers.FirstOrDefault(x => !x.Busy && x.TaxiPrice == order.Price);
            var taxi = Taxi.FirstOrDefault(x => x.ID == driver.TaxiID);
            driver.Busy = true;
            taxi.Busy = true;
            Drivers.Update(driver);
            Taxi.Update(taxi);
            order.DriverName = driver.Name;
            order.TaxiID = taxi.ID;
            Orders.Add(order);
            bankAccountContext.Users.FirstOrDefault(x => x.ID == order.UserID).HasOrder = true; // sets that definite user ordered taxi
            bankAccountContext.Withdraw(bankContext.BankAccounts.FirstOrDefault(x => x.UserBankAccountID == order.UserID), order.Price.GetHashCode());
            bankAccountContext.SaveChanges();
            SaveChanges();
        }

        /// <summary>
        /// updates data of user order
        /// </summary>
        /// <param name="order"></param>
        public void UpdateOrder(OrderModel order)
        {
            if (order is null)
                throw new ArgumentNullException();
            order.DriverName = Drivers.FirstOrDefault(x => !x.Busy).Name;
            order.Price = Orders.First(x => x.ID == order.ID).Price;
            ChangeTracker.Clear();
            Orders.Update(order);
            SaveChanges();
        }

        /// <summary>
        /// removes data of user order and accrual money on account
        /// </summary>
        /// <param name="order"></param>
        public void DeleteOrder(OrderModel order)
        {
            if (order is null)
                throw new ArgumentNullException();
            var driver = Drivers.FirstOrDefault(x => x.TaxiID == order.TaxiID);
            var taxi = Taxi.FirstOrDefault(x => x.ID == driver.TaxiID);
            driver.Busy = false;
            taxi.Busy = false;
            Drivers.Update(driver);
            Taxi.Update(taxi);
            Orders.Remove(order);
            bankAccountContext.Users.FirstOrDefault(x => x.ID == order.UserID).HasOrder = false;
            bankAccountContext.Accrual(bankContext.BankAccounts.FirstOrDefault(x => x.UserBankAccountID == order.UserID), order.Price.GetHashCode());
            bankAccountContext.SaveChanges();
            SaveChanges();
        }

        /// <summary>
        /// defines user already has order or not
        /// </summary>
        /// <param name="userID"></param>
        /// <returns><see langword="true"/> if the database there's order with the same user id</returns>
        public bool AlreadyOrder(Guid userID) => Orders.Any(x => x.UserID == userID);
    }
}
