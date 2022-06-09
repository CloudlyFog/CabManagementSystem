using CabManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CabManagementSystem.AppContext
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=localhost\\SQLEXPRESS;Data Source=maxim;Initial Catalog=CabManagementSystem;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False");
        }

        private readonly BankAccountContext bankAccountContext = new(new DbContextOptions<BankAccountContext>());
        public DbSet<OrderModel> Orders { get; set; }


        public void CreateOrder(OrderModel order)
        {
            Orders.Add(order);
            bankAccountContext.Withdraw(bankAccountContext.Users.FirstOrDefault(x => x.ID == order.UserID), (decimal)order.Price);
            SaveChanges();
        }

        public void UpdateOrder(OrderModel order)
        {
            Orders.Update(order);
            SaveChanges();
        }

        public void DeleteOrder(OrderModel order)
        {
            int price = int.Parse(order.Price.ToString());
            Orders.Remove(order);
            bankAccountContext.Users.FirstOrDefault(x => x.ID == order.UserID).HasOrder = false;
            bankAccountContext.Accrual(bankAccountContext.Users.FirstOrDefault(x => x.ID == order.UserID), (decimal)price);
            bankAccountContext.SaveChanges();
            SaveChanges();
        }

        public bool AlreadyOrder(Guid userID) => Orders.Any(x => x.UserID == userID);
    }
}
