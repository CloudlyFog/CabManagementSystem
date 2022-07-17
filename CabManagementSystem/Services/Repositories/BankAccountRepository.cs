using BankSystem.AppContext;
using BankSystem.Models;
using CabManagementSystem.Services.Interfaces;
using System.Linq.Expressions;

namespace CabManagementSystem.Services.Repositories
{
    public class BankAccountRepository : IBankAccountRepository<BankAccountModel>
    {
        private readonly BankAccountContext bankAccountContext;
        private const string queryConnection = @"Server=localhost\\SQLEXPRESS;Data Source=maxim;Initial Catalog=BankSystem;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False";
        public BankAccountRepository() => bankAccountContext = new(queryConnection);
        public BankAccountRepository(string connection) => bankAccountContext = new(connection);

        public ExceptionModel Accrual(BankAccountModel item, decimal amountAccrual) => bankAccountContext.Accrual(item, amountAccrual);

        public ExceptionModel Withdraw(BankAccountModel item, decimal amountAccrual) => bankAccountContext.Withdraw(item, amountAccrual);

        public ExceptionModel Update(BankAccountModel item, UserModel user) => bankAccountContext.UpdateBankAccount(item, user);

        public Models.ExceptionModel Create(BankAccountModel item) => (Models.ExceptionModel)bankAccountContext.AddBankAccount(item);

        public IEnumerable<BankAccountModel> Get() => bankAccountContext.BankAccounts.ToList();

        public BankAccountModel? Get(Guid id) => bankAccountContext.BankAccounts.FirstOrDefault(x => x.ID == id);

        public Models.ExceptionModel Update(BankAccountModel item) => Models.ExceptionModel.OperationRestricted;

        Models.ExceptionModel IRepository<BankAccountModel>.Delete(BankAccountModel item) => (Models.ExceptionModel)bankAccountContext.RemoveBankAccount(item);

        public bool Exist(Guid id) => bankAccountContext.BankAccounts.Any(x => x.ID == id);

        public BankAccountModel? Get(Expression<Func<BankAccountModel, bool>> predicate) => bankAccountContext.BankAccounts.FirstOrDefault(predicate);
    }
}
