﻿using CabManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CabManagementSystem.AppContext
{
    public class TaxiContext : DbContext
    {
        public TaxiContext(DbContextOptions<TaxiContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseSqlServer(
                @"Server=localhost\\SQLEXPRESS;Data Source=maxim;Initial Catalog=CabManagementSystem;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False");
        }

        public DbSet<TaxiModel> Taxi { get; set; }
        public DbSet<BindTaxiDriver> BindTaxiDriver { get; set; }
        public DbSet<DriverModel> Drivers { get; set; }
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
            taxi.TaxiClass = Taxi.Any(x => x.ID == taxi.ID)
                ? Taxi.FirstOrDefault(x => x.ID == taxi.ID).TaxiClass : taxi.TaxiClass;
            taxi.TaxiNumber = Taxi.Any(x => x.ID == taxi.ID)
                ? Taxi.FirstOrDefault(x => x.ID == taxi.ID).TaxiNumber : taxi.TaxiNumber;
            taxi.DriverID = Taxi.Any(x => x.ID == taxi.ID)
                ? Taxi.FirstOrDefault(x => x.ID == taxi.ID).DriverID : taxi.DriverID;
            taxi.SpecialName = Taxi.Any(x => x.ID == taxi.ID)
                ? Taxi.FirstOrDefault(x => x.ID == taxi.ID).SpecialName : taxi.SpecialName;
            //
            taxi.BindTaxiDriver.TaxiID = taxi.ID;
            taxi.BindTaxiDriver.DriverID = BindTaxiDriver.Any(x => x.TaxiID == taxi.ID)
                ? BindTaxiDriver.FirstOrDefault(x => x.TaxiID == taxi.ID).DriverID : new();
            taxi.BindTaxiDriver.ID = BindTaxiDriver.Any(x => x.TaxiID == taxi.ID)
                ? BindTaxiDriver.FirstOrDefault(x => x.TaxiID == taxi.ID).ID : new();
            //
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
        private void UpdateBindTaxiDriver(BindTaxiDriver bindTaxiDriver)
        {
            BindTaxiDriver.Update(bindTaxiDriver);
            SaveChanges();
        }
        private void DeleteBindTaxiDriver(BindTaxiDriver bindTaxiDriver)
        {
            BindTaxiDriver.Remove(bindTaxiDriver);
            SaveChanges();
        }

        public TaxiModel GetTaxi(Guid taxiID)
        {
            TaxiModel taxi = new();
            taxi.ID = taxiID;
            taxi.TaxiClass = Taxi.Any(x => x.ID == taxi.ID)
                ? Taxi.FirstOrDefault(x => x.ID == taxi.ID).TaxiClass : taxi.TaxiClass;
            taxi.TaxiNumber = Taxi.Any(x => x.ID == taxi.ID)
                ? Taxi.FirstOrDefault(x => x.ID == taxi.ID).TaxiNumber : taxi.TaxiNumber;
            taxi.DriverID = Taxi.Any(x => x.ID == taxi.ID)
                ? Taxi.FirstOrDefault(x => x.ID == taxi.ID).DriverID : taxi.DriverID;
            taxi.SpecialName = Taxi.Any(x => x.ID == taxi.ID)
                ? Taxi.FirstOrDefault(x => x.ID == taxi.ID).SpecialName : taxi.SpecialName;
            //
            taxi.BindTaxiDriver.TaxiID = taxi.ID;
            taxi.BindTaxiDriver.DriverID = BindTaxiDriver.Any(x => x.TaxiID == taxi.ID)
                ? BindTaxiDriver.FirstOrDefault(x => x.TaxiID == taxi.ID).DriverID : new();
            taxi.BindTaxiDriver.ID = BindTaxiDriver.Any(x => x.TaxiID == taxi.ID)
                ? BindTaxiDriver.FirstOrDefault(x => x.TaxiID == taxi.ID).ID : new();
            return taxi;
        }
    }
}
