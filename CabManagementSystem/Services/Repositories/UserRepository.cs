
using CabManagementSystem.AppContext;
using CabManagementSystem.Models;
using CabManagementSystem.Services.Interfaces;
using System.Linq.Expressions;

namespace CabManagementSystem.Services.Repositories
{
    public class UserRepository : IUserRepository<UserModel>
    {
        private readonly ApplicationContext applicationContext;
        private readonly BankSystem.AppContext.BankAccountContext bankAccountContext;
        public UserRepository()
        {
            applicationContext = new();
            bankAccountContext = new();
        }
        public ExceptionModel Create(UserModel item) => applicationContext.AddUser(item);

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
