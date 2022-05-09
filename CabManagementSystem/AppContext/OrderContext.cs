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

        public DbSet<OrderModel> Orders { get; set; }

        public void CreateOrder(OrderModel order)
        {
            Orders.Add(order);
            SaveChanges();
        }

        public void UpdateOrder(OrderModel order)
        {
            Orders.Update(order);
            SaveChanges();
        }

        public void DeleteOrder(OrderModel order)
        {
            Orders.Remove(order);
            SaveChanges();
        }

        public bool AlreadyOrder(Guid userID) => Orders.Any(x => x.UserID == userID);
    }
}
