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
        public ExceptionModel AddTaxi(TaxiModel taxi)
        {
            if (taxi is null)
                return ExceptionModel.VariableIsNull;
            if (taxi.BindTaxiDriver is null)
                return ExceptionModel.VariableIsNull;
            var operation = AddBindTaxiDriver(taxi.BindTaxiDriver);
            if (operation != ExceptionModel.Successfull)
                return operation;
            Taxi.Add(taxi);
            SaveChanges();
            return ExceptionModel.Successfull;
        }

        /// <summary>
        /// updates data of taxi in the database
        /// </summary>
        /// <param name="taxi"></param>
        public ExceptionModel UpdateTaxi(TaxiModel taxi)
        {
            if (taxi is null)
                return ExceptionModel.VariableIsNull;
            Taxi.Update(taxi);
            SaveChanges();
            return ExceptionModel.Successfull;
        }

        /// <summary>
        /// removes data of taxi in the database
        /// </summary>
        /// <param name="taxi"></param>
        public ExceptionModel DeleteTaxi(TaxiModel taxi)
        {
            taxi = Taxi.FirstOrDefault(x => x.ID == taxi.ID);
            ChangeTracker.Clear();
            if (taxi is null)
                return ExceptionModel.VariableIsNull;

            if (taxi.BindTaxiDriver is null)
                return ExceptionModel.VariableIsNull;
            var operation = DeleteBindTaxiDriver(taxi.BindTaxiDriver);
            if (operation != ExceptionModel.Successfull)
                return operation;
            Taxi.Remove(taxi);
            SaveChanges();
            return ExceptionModel.Successfull;
        }

        /// <summary>
        /// adds bind's data of taxi and its driver in the database
        /// </summary>
        /// <param name="bindTaxiDriver"></param>
        private ExceptionModel AddBindTaxiDriver(BindTaxiDriver bindTaxiDriver)
        {
            if (bindTaxiDriver is null)
                return ExceptionModel.VariableIsNull;
            BindTaxiDriver.Add(bindTaxiDriver);
            SaveChanges();
            return ExceptionModel.Successfull;
        }

        /// <summary>
        /// removes bind's data of taxi and its driver in the database
        /// </summary>
        /// <param name="bindTaxiDriver"></param>
        private ExceptionModel DeleteBindTaxiDriver(BindTaxiDriver bindTaxiDriver)
        {
            if (bindTaxiDriver is null)
                return ExceptionModel.VariableIsNull;
            BindTaxiDriver.Remove(bindTaxiDriver);
            SaveChanges();
            return ExceptionModel.Successfull;
        }
    }
}
