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

        /// <summary>
        /// adds data of taxi in the database
        /// </summary>
        /// <param name="taxi"></param>
        public void AddTaxi(TaxiModel taxi)
        {
            AddBindTaxiDriver(taxi.BindTaxiDriver);
            Taxi.Add(taxi);
            SaveChanges();
        }

        /// <summary>
        /// updates data of taxi in the database
        /// </summary>
        /// <param name="taxi"></param>
        public void UpdateTaxi(TaxiModel taxi)
        {
            Taxi.Update(taxi);
            SaveChanges();
        }

        /// <summary>
        /// removes data of taxi in the database
        /// </summary>
        /// <param name="taxi"></param>
        public void DeleteTaxi(TaxiModel taxi)
        {
            taxi = Taxi.Any(x => x.ID == taxi.ID)
                ? Taxi.First(x => x.ID == taxi.ID) : new();
            ChangeTracker.Clear();
            DeleteBindTaxiDriver(taxi.BindTaxiDriver);
            Taxi.Remove(taxi);
            SaveChanges();
        }

        /// <summary>
        /// adds bind's data of taxi and its driver in the database
        /// </summary>
        /// <param name="bindTaxiDriver"></param>
        private void AddBindTaxiDriver(BindTaxiDriver bindTaxiDriver)
        {
            BindTaxiDriver.Add(bindTaxiDriver);
            SaveChanges();
        }

        /// <summary>
        /// removes bind's data of taxi and its driver in the database
        /// </summary>
        /// <param name="bindTaxiDriver"></param>
        private void DeleteBindTaxiDriver(BindTaxiDriver bindTaxiDriver)
        {
            BindTaxiDriver.Remove(bindTaxiDriver);
            SaveChanges();
        }

    }
}
