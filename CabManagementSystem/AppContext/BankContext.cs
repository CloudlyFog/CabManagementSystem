using CabManagementSystem.Models;
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

        public void CreateOperation(OperationModel operationModel, OperationKind operationKind)
        {
            operationModel.OperationStatus = StatusOperation(operationModel, operationKind);
            Operations.Add(operationModel);
            SaveChanges();
        }
        public void DeleteOperation(OperationModel operationModel)
        {
            if (operationModel is null)
                throw new ArgumentNullException();
            if (Operations.Any(x => x.ID == operationModel.ID))
                Operations.Remove(operationModel);
            SaveChanges();
        }


        public void BankAccrual(UserModel user, BankModel bankModel, OperationModel operationModel)
        {
            if (StatusOperation(operationModel, operationModel.OperationKind) != StatusOperationCode.Successfull)
                throw new Exception($"operation status is {operationModel.OperationStatus}");
            bankModel.AccountAmount -= operationModel.TransferAmount;
            user.BankAccountAmount += operationModel.TransferAmount;
            Banks.Update(bankModel);
            Users.Update(user);
            SaveChanges();
        }

        public void BankWithdraw(UserModel user, BankModel bankModel, OperationModel operationModel)
        {
            if (StatusOperation(operationModel, operationModel.OperationKind) != StatusOperationCode.Successfull)
                throw new Exception($"operation status is {operationModel.OperationStatus}");
            bankModel.AccountAmount += operationModel.TransferAmount;
            user.BankAccountAmount -= operationModel.TransferAmount;
            Banks.Update(bankModel);
            Users.Update(user);
            SaveChanges();
        }

        /// <summary>
        /// check: 
        /// 1) is exist user with the same ID and bank with the same BankID as a sender or reciever in the database.
        /// 2) is exist bank with the same BankID as a single bank.
        /// 3) is bank's money enough for transaction.
        /// </summary>
        /// <param name="operationModel"></param>
        /// <param name="operationKind"></param>
        /// <returns>status of operation, default - successfull</returns>
        /// <exception cref="ArgumentNullException"></exception>
        private StatusOperationCode StatusOperation(OperationModel operationModel, OperationKind operationKind)
        {
            if (operationModel is null)
                throw new ArgumentNullException();

            if (operationKind == OperationKind.Accrual)
            {
                if (!Banks.Any(x => x.BankID == operationModel.SenderID) || !Users.Any(x => x.ID == operationModel.ReceiverID))
                    operationModel.OperationStatus = StatusOperationCode.Error;
            }
            else
            {
                if (!Banks.Any(x => x.BankID == operationModel.ReceiverID) || !Users.Any(x => x.ID == operationModel.SenderID))
                    operationModel.OperationStatus = StatusOperationCode.Error;
            }

            if (!Banks.Any(x => x.BankID == operationModel.BankID))
                operationModel.OperationStatus = StatusOperationCode.Error;

            if (Banks.FirstOrDefault(x => x.BankID == operationModel.SenderID)?.AccountAmount < operationModel.TransferAmount)
                operationModel.OperationStatus = StatusOperationCode.Error;
            return operationModel.OperationStatus;
        }
    }
}
