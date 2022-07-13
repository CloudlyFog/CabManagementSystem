
using CabManagementSystem.AppContext;
using CabManagementSystem.Models;
using RestApiCMS.Services.Interfaces;

namespace RestApiCMS.Services.Repositories
{
    public class UserRepository : IRepository<UserModel>
    {
        private readonly ApplicationContext applicationContext;
        private readonly BankSystem.AppContext.BankAccountContext bankAccountContext;
        public UserRepository()
        {
            applicationContext = new();
            bankAccountContext = new();
        }
        public ExceptionModel Create(UserModel item)
        {
            if (Get(item.ID) is null)//if user isn`t exist method will exit with exception OperationFailed
                return ExceptionModel.OperationFailed;
            item.Password = applicationContext.HashPassword(item.Password);
            item.Authenticated = true;
            item.ID = Guid.NewGuid();
            applicationContext.Users.Add(item);
            applicationContext.SaveChanges();
            var bankAccountModel = new BankSystem.Models.BankAccountModel()
            {
                ID = item.BankAccountID,
                UserBankAccountID = item.ID
            };
            return (ExceptionModel)bankAccountContext.AddBankAccount(bankAccountModel);
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
    }
}
