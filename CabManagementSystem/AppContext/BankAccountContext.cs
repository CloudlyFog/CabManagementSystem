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

        public DbSet<BankModel> Banks { get; set; }
        public DbSet<UserModel> Users { get; set; }

        /// <summary>
        /// accrual money on account with the same user id
        /// </summary>
        /// <param name="user"></param>
        /// <param name="amountAccrual"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Accrual(UserModel user, decimal amountAccrual)
        {
            if (user is null || !Users.Any(x => x.ID == user.ID))
                throw new ArgumentNullException();
            var userAcc = Users.First(x => x.ID == user.ID);
            userAcc.BankAccountAmount += amountAccrual;
            SaveChanges();
        }

        /// <summary>
        /// withdraw money from account with the same user id
        /// </summary>
        /// <param name="user"></param>
        /// <param name="amountWithdraw"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void Withdraw(UserModel user, decimal amountWithdraw)
        {
            if (user is null || !Users.Any(x => x.ID == user.ID))
                throw new ArgumentNullException();
            if (user.BankAccountAmount < amountWithdraw)
                throw new ArgumentException();
            user.BankAccountAmount -= amountWithdraw;
            SaveChanges();
        }
    }
}
