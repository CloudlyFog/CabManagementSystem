
using CabManagementSystem.AppContext;
using CabManagementSystem.Models;
using CabManagementSystem.Services.Interfaces;
using System.Linq.Expressions;
using BankAccountModel = BankSystem.Models.BankAccountModel;

namespace CabManagementSystem.Services.Repositories
{
    public class UserRepository : IUserRepository<UserModel>
    {
        private readonly ApplicationContext applicationContext;
        private readonly IBankAccountRepository<BankAccountModel> bankAccountRepository;
        public UserRepository()
        {
            applicationContext = new();
            bankAccountRepository = new BankAccountRepository();
        }
        public ExceptionModel Create(UserModel item)
        {
            if (Exist(item.ID))//if user isn`t exist method will send false
                return ExceptionModel.OperationFailed;
            item.Password = applicationContext.HashPassword(item.Password);
            item.Authenticated = true;
            item.ID = Guid.NewGuid();
            applicationContext.Users.Add(item);
            applicationContext.SaveChanges();
            var bankAccountModel = new BankAccountModel()
            {
                ID = item.BankAccountID,
                UserBankAccountID = item.ID
            };
            return bankAccountRepository.Create(bankAccountModel);
        }

        public ExceptionModel Delete(UserModel item)
        {
            if (item is null)
                return ExceptionModel.OperationFailed;
            if (Get(item.ID) is null)
                return ExceptionModel.OperationFailed;
            applicationContext.Users.Remove(item);
            applicationContext.SaveChanges();
            var bankAccountModel = bankAccountContext.BankAccounts.FirstOrDefault(x => x.UserBankAccountID == item.ID);
            return (ExceptionModel)bankAccountContext.RemoveBankAccount(bankAccountModel);
        }

        public bool Exist(Guid id) => applicationContext.Users.Any(user => user.ID == id && user.Authenticated);

        public IEnumerable<UserModel> Get() => applicationContext.Users.Any() ? applicationContext.Users.AsEnumerable() : new List<UserModel>();

        public UserModel Get(Guid id) => applicationContext.Users.Any(x => x.ID == id) ? applicationContext.Users.First(x => x.ID == id) : new UserModel();

        public UserModel? Get(Expression<Func<UserModel, bool>> predicate) => applicationContext.Users.FirstOrDefault(predicate);

        public ExceptionModel Update(UserModel item)
        {
            if (item is null)
                return ExceptionModel.OperationFailed;
            if (Get(item.ID) is null)
                return ExceptionModel.OperationFailed;
            applicationContext.Users.Update(item);
            applicationContext.SaveChanges();
            return ExceptionModel.Successfull;
        }

        string IUserRepository<UserModel>.HashPassword(string password) => applicationContext.HashPassword(password);
    }
}
