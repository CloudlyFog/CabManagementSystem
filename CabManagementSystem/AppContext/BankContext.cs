﻿using CabManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CabManagementSystem.AppContext
{
    public class BankContext : DbContext
    {
        public BankContext(DbContextOptions<BankContext> options) : base(options) => Database.EnsureCreated();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseSqlServer(
                @"Server=localhost\\SQLEXPRESS;Data Source=maxim;Initial Catalog=CabManagementSystem;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False");
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<BankModel> Banks { get; set; }
        public DbSet<OperationModel> Operations { get; set; }
        public DbSet<BankAccountModel> BankAccounts { get; set; }

        /// <summary>
        /// creates transaction operation
        /// </summary>
        /// <param name="operationModel"></param>
        /// <param name="operationKind"></param>
        public ExceptionModel CreateOperation(OperationModel operationModel, OperationKind operationKind)
        {
            if (operationModel is null)
                return ExceptionModel.VariableIsNull;
            operationModel.OperationStatus = StatusOperation(operationModel, operationKind);
            Operations.Add(operationModel);
            SaveChanges();
            return ExceptionModel.Successfull;
        }

        /// <summary>
        /// delete transaction operation
        /// </summary>
        /// <param name="operationModel"></param>
        /// <exception cref="ArgumentNullException"></exception>
        private ExceptionModel DeleteOperation(OperationModel operationModel)
        {
            if (operationModel is null)
                return ExceptionModel.VariableIsNull;
            if (!Operations.Any(x => x.ID == operationModel.ID))
                return ExceptionModel.OperationNotExist;
            Operations.Remove(operationModel);
            SaveChanges();
            return ExceptionModel.Successfull;
        }

        /// <summary>
        /// accrual money to user bank account from bank's account
        /// </summary>
        /// <param name="bankAccountModel"></param>
        /// <param name="bankModel"></param>
        /// <param name="operationModel"></param>
        /// <exception cref="Exception"></exception>
        public ExceptionModel BankAccrual(BankAccountModel bankAccountModel, BankModel bankModel, OperationModel operationModel)
        {
            if (bankAccountModel is null)
                return ExceptionModel.VariableIsNull;
            if (bankModel is null)
                return ExceptionModel.VariableIsNull;
            if (operationModel.OperationStatus != StatusOperationCode.Successfull)
                return (ExceptionModel)operationModel.OperationStatus.GetHashCode();

            var user = Users.FirstOrDefault(x => x.ID == bankAccountModel.UserBankAccountID);
            if (user is null)
                return ExceptionModel.VariableIsNull;

            bankModel.AccountAmount -= operationModel.TransferAmount;
            bankAccountModel.BankAccountAmount += operationModel.TransferAmount;
            user.BankAccountAmount = bankAccountModel.BankAccountAmount;
            ChangeTracker.Clear();
            BankAccounts.Update(bankAccountModel);
            Banks.Update(bankModel);
            Users.Update(user);
            SaveChanges();
            DeleteOperation(operationModel);
            return ExceptionModel.Successfull;
        }

        /// <summary>
        /// withdraw money from user bank account and accrual to bank's account
        /// </summary>
        /// <param name="user"></param>
        /// <param name="bankModel"></param>
        /// <param name="operationModel"></param>
        /// <exception cref="Exception"></exception>
        public ExceptionModel BankWithdraw(BankAccountModel bankAccountModel, BankModel bankModel, OperationModel operationModel)
        {
            if (bankAccountModel is null)
                return ExceptionModel.VariableIsNull;
            if (bankModel is null)
                return ExceptionModel.VariableIsNull;
            if (operationModel.OperationStatus != StatusOperationCode.Successfull)
                return (ExceptionModel)operationModel.OperationStatus.GetHashCode();

            var user = Users.FirstOrDefault(x => x.ID == bankAccountModel.UserBankAccountID);
            if (user is null)
                return ExceptionModel.VariableIsNull;

            bankModel.AccountAmount += operationModel.TransferAmount;
            bankAccountModel.BankAccountAmount -= operationModel.TransferAmount;
            user.BankAccountAmount = bankAccountModel.BankAccountAmount;
            ChangeTracker.Clear();
            BankAccounts.Update(bankAccountModel);
            Banks.Update(bankModel);
            Users.Update(user);
            SaveChanges();
            DeleteOperation(operationModel);
            return ExceptionModel.Successfull;
        }

        /// <summary>
        /// check: 
        /// 1) is exist user with the same ID and bank with the same BankID as a sender or reciever in the database.
        /// 2) is exist bank with the same BankID as a single bank.
        /// 3) is bank's money enough for transaction.
        /// 4) is user's money enough for transaction.
        /// </summary>
        /// <param name="operationModel"></param>
        /// <param name="operationKind"></param>
        /// <returns>status of operation, default - successfull</returns>
        /// <exception cref="ArgumentNullException"></exception>
        private StatusOperationCode StatusOperation(OperationModel operationModel, OperationKind operationKind)
        {
            if (operationModel is null)
                return StatusOperationCode.Error;

            if (operationKind == OperationKind.Accrual)
            {
                // SenderID is ID of bank
                // ReceiverID is ID of user
                if (!Banks.Any(x => x.BankID == operationModel.SenderID) || !Users.Any(x => x.ID == operationModel.ReceiverID))
                    operationModel.OperationStatus = StatusOperationCode.Error;

                if (Banks.FirstOrDefault(x => x.BankID == operationModel.SenderID)?.AccountAmount < operationModel.TransferAmount)
                    operationModel.OperationStatus = StatusOperationCode.Restricted;
            }
            else
            {
                // SenderID is ID of user
                // ReceiverID is ID of bank
                if (!Banks.Any(x => x.BankID == operationModel.ReceiverID) || !Users.Any(x => x.ID == operationModel.SenderID))
                    operationModel.OperationStatus = StatusOperationCode.Error;
                if (BankAccounts.FirstOrDefault(x => x.UserBankAccountID == operationModel.SenderID)?.BankAccountAmount < operationModel.TransferAmount)
                    operationModel.OperationStatus = StatusOperationCode.Restricted;
            }

            if (!Banks.Any(x => x.BankID == operationModel.BankID))
                operationModel.OperationStatus = StatusOperationCode.Error;

            return operationModel.OperationStatus;
        }
    }
}
