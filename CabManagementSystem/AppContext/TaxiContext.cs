using CabManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CabManagementSystem.AppContext
{
    public class TaxiContext : DbContext
    {
        public TaxiContext() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseSqlServer(
                @"Server=localhost\\SQLEXPRESS;Data Source=maxim;Initial Catalog=CabManagementSystem;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False");
        }

        internal protected DbSet<TaxiModel> Taxi { get; set; }
        internal protected DbSet<BindTaxiDriver> BindTaxiDriver { get; set; }

        /// <summary>
        /// adds bind's data of taxi and its driver in the database
        /// </summary>
        /// <param name="bindTaxiDriver"></param>
        internal protected ExceptionModel AddBindTaxiDriver(BindTaxiDriver bindTaxiDriver)
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
        internal protected ExceptionModel DeleteBindTaxiDriver(BindTaxiDriver bindTaxiDriver)
        {
            if (bindTaxiDriver is null)
                return ExceptionModel.VariableIsNull;
            BindTaxiDriver.Remove(bindTaxiDriver);
            SaveChanges();
            return ExceptionModel.Successfull;
        }
    }
}
