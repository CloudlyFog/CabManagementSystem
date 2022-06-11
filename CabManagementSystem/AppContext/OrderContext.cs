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

        public void CreateOrder(OrderModel order)
        {
            order.DriverName = Drivers.FirstOrDefault(x => !x.Busy).Name;
            Orders.Add(order);
            bankAccountContext.Users.FirstOrDefault(x => x.ID == order.UserID).HasOrder = true;
            bankAccountContext.Withdraw(bankAccountContext.Users.FirstOrDefault(x => x.ID == order.UserID), order.Price.GetHashCode());
            SaveChanges();
        }

        public void UpdateOrder(OrderModel order)
        {
            Orders.Update(order);
            SaveChanges();
        }

        public void DeleteOrder(OrderModel order)
        {
            var price = decimal.Parse(order.Price.GetHashCode().ToString());
            Orders.Remove(order);
            bankAccountContext.Users.FirstOrDefault(x => x.ID == order.UserID).HasOrder = false;
            bankAccountContext.Accrual(bankAccountContext.Users.FirstOrDefault(x => x.ID == order.UserID), price);
            bankAccountContext.SaveChanges();
            SaveChanges();
        }

        public bool AlreadyOrder(Guid userID) => Orders.Any(x => x.UserID == userID);
    }
}
