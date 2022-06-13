﻿using CabManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CabManagementSystem.AppContext
{
    public class BankAccountContext : DbContext
    {
        public BankAccountContext(DbContextOptions<BankAccountContext> options) : base(options) => Database.EnsureCreated();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseSqlServer(
                @"Server=localhost\\SQLEXPRESS;Data Source=maxim;Initial Catalog=CabManagementSystem;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False");
        }

        public DbSet<UserModel> Users { get; set; }
        private readonly BankContext bankContext = new(new DbContextOptions<BankContext>());

        public void Accrual(UserModel user, decimal amountAccrual)
        {
            if (user is null || !Users.Any(x => x.ID == user.ID))
                throw new ArgumentNullException();

            var operation = new OperationModel()
            {
                BankID = user.BankID,
                SenderID = user.BankID,
                ReceiverID = user.ID,
                TransferAmount = amountAccrual,
                OperationKind = OperationKind.Accrual
            };
            bankContext.CreateOperation(operation, OperationKind.Accrual);
            bankContext.BankAccrual(user, bankContext.Banks.FirstOrDefault(x => x.BankID == operation.BankID), operation);
        }
        public void Withdraw(UserModel user, decimal amountWithdraw)
        {
            if (user is null || !Users.Any(x => x.ID == user.ID))
                throw new ArgumentNullException();

            var operation = new OperationModel()
            {
                BankID = user.BankID,
                SenderID = user.ID,
                ReceiverID = user.BankID,
                TransferAmount = amountWithdraw,
                OperationKind = OperationKind.Withdraw
            };
            bankContext.CreateOperation(operation, OperationKind.Withdraw);
            bankContext.BankWithdraw(user, bankContext.Banks.FirstOrDefault(x => x.BankID == operation.BankID), operation);
        }
    }
}
