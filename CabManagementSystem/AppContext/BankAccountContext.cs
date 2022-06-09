using CabManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CabManagementSystem.AppContext
{
    public class BankAccountContext : DbContext
    {
        public BankAccountContext(DbContextOptions<BankAccountContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseSqlServer(
                @"Server=localhost\\SQLEXPRESS;Data Source=maxim;Initial Catalog=CabManagementSystem;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False");
        }

        public DbSet<BankModel> Banks { get; set; }
        public DbSet<UserModel> Users { get; set; }

        public void Accrual(UserModel user, decimal amountAccrual)
        {
            if (user is null || !Users.Any(x => x.ID == user.ID))
                throw new ArgumentNullException();
            var userAcc = Users.First(x => x.ID == user.ID);
            userAcc.BankAccountAmount += amountAccrual;
            SaveChanges();
        }
        public void Withdraw(UserModel user, decimal amountWithdraw)
        {
            if (user is null || !Users.Any(x => x.ID == user.ID))
                throw new ArgumentNullException();
            var userAcc = Users.First(x => x.ID == user.ID);
            if (userAcc.BankAccountAmount < amountWithdraw)
                throw new ArgumentException();
            userAcc.BankAccountAmount -= amountWithdraw;
            SaveChanges();
        }
    }
}
