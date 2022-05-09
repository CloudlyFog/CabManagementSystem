using CabManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CabManagementSystem.AppContext
{
    public class TaxiContext : DbContext
    {
        public TaxiContext(DbContextOptions<OrderContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=localhost\\SQLEXPRESS;Data Source=maxim;Initial Catalog=CabManagementSystem;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False");
        }

        public DbSet<TaxiModel> Taxi { get; set; }
        public void AddTaxi(TaxiModel taxi)
        {
            Taxi.Add(taxi);
            SaveChanges();
        }
        public void UpdateTaxi(TaxiModel taxi)
        {
            Taxi.Update(taxi);
            SaveChanges();
        }
        public void DeleteTaxi(TaxiModel taxi)
        {
            Taxi.Remove(taxi);
            SaveChanges();
        }
    }
}
