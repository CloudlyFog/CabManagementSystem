using BankSystem.AppContext;
using BankSystem.Models;
using RestApiCMS.Services.Interfaces;

namespace RestApiCMS.Services.Repositories
{
    public class BankAccountRepository : IBankAccountRepository<BankAccountModel>
    {
        private readonly BankAccountContext bankAccountContext;
        private const string queryConnection = @"Server=localhost\\SQLEXPRESS;Data Source=maxim;Initial Catalog=BankSystem;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False";
        public BankAccountRepository() => bankAccountContext = new(queryConnection);
        public BankAccountRepository(string queryConnectionUser) => bankAccountContext = new(queryConnectionUser);

        public ExceptionModel Accrual(BankAccountModel item, decimal amountAccrual) => bankAccountContext.Accrual(item, amountAccrual);

        public ExceptionModel Withdraw(BankAccountModel item, decimal amountAccrual) => bankAccountContext.Withdraw(item, amountAccrual);

        public ExceptionModel Update(BankAccountModel item, UserModel user) => bankAccountContext.UpdateBankAccount(item, user);

        public CabManagementSystem.Models.ExceptionModel Create(BankAccountModel item) => (CabManagementSystem.Models.ExceptionModel)bankAccountContext.AddBankAccount(item);

        public IEnumerable<BankAccountModel> Get() => bankAccountContext.BankAccounts.ToList();

        public BankAccountModel Get(Guid id) => bankAccountContext.BankAccounts.FirstOrDefault(x => x.ID == id);

        public CabManagementSystem.Models.ExceptionModel Update(BankAccountModel item) => CabManagementSystem.Models.ExceptionModel.OperationRestricted;

        CabManagementSystem.Models.ExceptionModel IRepository<BankAccountModel>.Delete(BankAccountModel item) => (CabManagementSystem.Models.ExceptionModel)bankAccountContext.RemoveBankAccount(item);

        public bool Exist(Guid id) => bankAccountContext.BankAccounts.Any(x => x.ID == id);

    }
}
