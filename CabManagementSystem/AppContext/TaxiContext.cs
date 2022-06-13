using CabManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CabManagementSystem.AppContext
{
    public class TaxiContext : DbContext
    {
        public TaxiContext(DbContextOptions<TaxiContext> options) : base(options) => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseSqlServer(
                @"Server=localhost\\SQLEXPRESS;Data Source=maxim;Initial Catalog=CabManagementSystem;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False");
        }

        public DbSet<TaxiModel> Taxi { get; set; }
        public DbSet<BindTaxiDriver> BindTaxiDriver { get; set; }
        public void AddTaxi(TaxiModel taxi)
        {
            AddBindTaxiDriver(taxi.BindTaxiDriver);
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
            taxi = Taxi.Any(x => x.ID == taxi.ID)
                ? Taxi.First(x => x.ID == taxi.ID) : new();
            ChangeTracker.Clear();
            DeleteBindTaxiDriver(taxi.BindTaxiDriver);
            Taxi.Remove(taxi);
            SaveChanges();
        }

        private void AddBindTaxiDriver(BindTaxiDriver bindTaxiDriver)
        {
            BindTaxiDriver.Add(bindTaxiDriver);
            SaveChanges();
        }

        private void DeleteBindTaxiDriver(BindTaxiDriver bindTaxiDriver)
        {
            BindTaxiDriver.Remove(bindTaxiDriver);
            SaveChanges();
        }
    }
}
