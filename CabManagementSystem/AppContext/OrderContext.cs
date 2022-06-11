using CabManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CabManagementSystem.AppContext
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options) => Database.EnsureCreated();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=localhost\\SQLEXPRESS;Data Source=maxim;Initial Catalog=CabManagementSystem;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False");
        }

        private readonly BankAccountContext bankAccountContext = new(new DbContextOptions<BankAccountContext>());
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<DriverModel> Drivers { get; set; }

        /// <summary>
        /// adds data of user order and withdraw money from account
        /// </summary>
        /// <param name="order"></param>
        public void CreateOrder(OrderModel order)
        {
            order.DriverName = Drivers.FirstOrDefault(x => !x.Busy).Name;
            Orders.Add(order);
            bankAccountContext.Users.FirstOrDefault(x => x.ID == order.UserID).HasOrder = true; // sets that definite user ordered taxi
            bankAccountContext.Withdraw(bankAccountContext.Users.FirstOrDefault(x => x.ID == order.UserID), order.Price.GetHashCode());
            SaveChanges();
        }

        /// <summary>
        /// updates data of user order
        /// </summary>
        /// <param name="order"></param>
        public void UpdateOrder(OrderModel order)
        {
            Orders.Update(order);
            SaveChanges();
        }

        /// <summary>
        /// removes data of user order and accrual money on account
        /// </summary>
        /// <param name="order"></param>
        public void DeleteOrder(OrderModel order)
        {
            var price = decimal.Parse(order.Price.GetHashCode().ToString());
            Orders.Remove(order);
            bankAccountContext.Users.FirstOrDefault(x => x.ID == order.UserID).HasOrder = false;
            bankAccountContext.Accrual(bankAccountContext.Users.FirstOrDefault(x => x.ID == order.UserID), price);
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
