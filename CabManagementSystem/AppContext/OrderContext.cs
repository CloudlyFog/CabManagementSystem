using CabManagementSystem.Models;

namespace CabManagementSystem.AppContext
{
    public class OrderContext : DbContext
    {
        public OrderContext() => Database.EnsureCreated();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseSqlServer(
                @"Server=localhost\\SQLEXPRESS;Data Source=maxim;Initial Catalog=CabManagementSystem;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False");
        }
        internal protected DbSet<OrderModel> Orders { get; set; }
        internal protected DbSet<DriverModel> Drivers { get; set; }
        internal protected DbSet<TaxiModel> Taxi { get; set; }

        /// <summary>
        /// defines user already has order or not
        /// </summary>
        /// <param name="userID"></param>
        /// <returns><see langword="true"/> if the database there's order with the same user id</returns>
        internal protected bool AlreadyOrder(Guid userID) => Orders.Any(x => x.UserID == userID);
    }
}
