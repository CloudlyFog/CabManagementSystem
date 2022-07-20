using BankSystem.AppContext;
using BankSystem.Models;
using CabManagementSystem.Services.Interfaces;
using System.Linq.Expressions;

namespace CabManagementSystem.Services.Repositories
{
    public class BankAccountRepository : BankAccountContext, IBankAccountRepository<BankAccountModel>
    {
        private readonly BankAccountContext bankAccountContext;
        private const string queryConnection = @"Server=localhost\\SQLEXPRESS;Data Source=maxim;Initial Catalog=BankSystem;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False";
        public BankAccountRepository() => bankAccountContext = new(queryConnection);
        public BankAccountRepository(string connection) => bankAccountContext = new(connection);

        public ExceptionModel Accrual(BankAccountModel item, decimal amountAccrual) => bankAccountContext.Accrual(item, amountAccrual);

        public ExceptionModel Withdraw(BankAccountModel item, decimal amountAccrual) => bankAccountContext.Withdraw(item, amountAccrual);

        public ExceptionModel Update(BankAccountModel item, UserModel user) => bankAccountContext.UpdateBankAccount(item, user);

        public Models.ExceptionModel Create(BankAccountModel item) => (Models.ExceptionModel)bankAccountContext.AddBankAccount(item);

        public IEnumerable<BankAccountModel> Get() => BankAccounts;

        public BankAccountModel? Get(Guid id) => BankAccounts.Any(x => x.ID == id) ? BankAccounts.FirstOrDefault(x => x.ID == id) : new();

        public Models.ExceptionModel Update(BankAccountModel item)
        {
            if (item is null)
                return Models.ExceptionModel.OperationFailed;
            if (!Exist(x => x.ID == item.ID))
                return Models.ExceptionModel.OperationFailed;
            Update(item);
            SaveChanges();
            return Models.ExceptionModel.Successfull;
        }

        Models.ExceptionModel IRepository<BankAccountModel>.Delete(BankAccountModel item) => (Models.ExceptionModel)bankAccountContext.RemoveBankAccount(item);

        public bool Exist(Guid id) => BankAccounts.Any(x => x.ID == id);

        public BankAccountModel? Get(Expression<Func<BankAccountModel, bool>> predicate) => BankAccounts.Any(predicate) ? BankAccounts.FirstOrDefault(predicate) : new();

        public bool Exist(Expression<Func<BankAccountModel, bool>> predicate) => BankAccounts.Any(predicate);
    }
}
