using CabManagementSystem.Models;
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
        public DbSet<BankAccountModel> BankAccounts { get; set; }
        private readonly BankContext bankContext = new(new DbContextOptions<BankContext>());

        public void AddBankAccount(BankAccountModel bankAccountModel)
        {
            if (bankAccountModel is null)
                throw new ArgumentNullException();
            BankAccounts.Add(bankAccountModel);
            SaveChanges();
        }

        /// <summary>
        /// accrual money on account with the same user id
        /// </summary>
        /// <param name="BankAccountModel"></param>
        /// <param name="amountAccrual"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Accrual(BankAccountModel BankAccountModel, decimal amountAccrual)
        {
            if (BankAccountModel is null || !Users.Any(x => x.ID == BankAccountModel.UserBankAccountID))
                throw new ArgumentNullException();

            var operation = new OperationModel()
            {
                BankID = BankAccountModel.BankID,
                SenderID = BankAccountModel.BankID,
                ReceiverID = BankAccountModel.UserBankAccountID,
                TransferAmount = amountAccrual,
                OperationKind = OperationKind.Accrual
            };
            bankContext.CreateOperation(operation, OperationKind.Accrual);
            bankContext.BankAccrual(BankAccountModel, bankContext.Banks.FirstOrDefault(x => x.BankID == operation.BankID), operation);
        }

        /// <summary>
        /// withdraw money from account with the same user id
        /// </summary>
        /// <param name="bankAccountModel"></param>
        /// <param name="amountWithdraw"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void Withdraw(BankAccountModel bankAccountModel, decimal amountWithdraw)
        {
            if (bankAccountModel is null || !Users.Any(x => x.ID == bankAccountModel.UserBankAccountID))
                throw new ArgumentNullException();

            var operation = new OperationModel()
            {
                BankID = bankAccountModel.BankID,
                SenderID = bankAccountModel.UserBankAccountID,
                ReceiverID = bankAccountModel.BankID,
                TransferAmount = amountWithdraw,
                OperationKind = OperationKind.Withdraw
            };
            bankContext.CreateOperation(operation, OperationKind.Withdraw);
            bankContext.BankWithdraw(bankAccountModel, bankContext.Banks.FirstOrDefault(x => x.BankID == operation.BankID), operation);
        }
    }
}
