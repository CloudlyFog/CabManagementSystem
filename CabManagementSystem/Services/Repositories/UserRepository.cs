
using CabManagementSystem.AppContext;
using CabManagementSystem.Models;
using CabManagementSystem.Services.Interfaces;
using System.Linq.Expressions;
using BankAccountModel = BankSystem.Models.BankAccountModel;

namespace CabManagementSystem.Services.Repositories
{
    public class UserRepository : ApplicationContext, IUserRepository<UserModel>
    {
<<<<<<< HEAD
        private readonly BankSystem.Services.Interfaces.IBankAccountRepository<BankAccountModel> bankAccountRepository;
        public UserRepository()
        {
            bankAccountRepository = new BankSystem.Services.Repositories.BankAccountRepository();
        }
=======
        private readonly IBankAccountRepository<BankAccountModel> bankAccountRepository;
        public UserRepository() => bankAccountRepository = new BankAccountRepository();
        public UserRepository(string connection) => bankAccountRepository = new BankAccountRepository(connection);
>>>>>>> 2a8999de2f8e1524e53d22323eae746fbc609fa8

        public ExceptionModel Create(UserModel item)
        {
            if (Exist(item.ID))//if user isn`t exist method will send false
                return ExceptionModel.OperationFailed;
            item.Password = HashPassword(item.Password);
            item.Authenticated = true;
            item.ID = Guid.NewGuid();
            Users.Add(item);
            SaveChanges();
            var bankAccountModel = new BankAccountModel()
            {
                ID = item.BankAccountID,
                UserBankAccountID = item.ID
            };
            return (ExceptionModel)bankAccountRepository.Create(bankAccountModel);
        }

        public ExceptionModel Delete(UserModel item)
        {
            if (item is null)
                return ExceptionModel.OperationFailed;
            if (Get(item.ID) is null)
                return ExceptionModel.OperationFailed;
            Users.Remove(item);
            SaveChanges();
            var bankAccountModel = bankAccountRepository.Get(x => x.UserBankAccountID == item.ID);
            return (ExceptionModel)bankAccountRepository.Delete(bankAccountModel);
        }

        public bool Exist(Guid id) => Users.Any(user => user.ID == id && user.Authenticated);

        public bool Exist(Expression<Func<UserModel, bool>> predicate) => Users.Any(predicate);

        public IEnumerable<UserModel> Get() => Users.Any() ? Users.AsEnumerable() : new List<UserModel>();

        public UserModel Get(Guid id) => Users.Any(x => x.ID == id) ? Users.First(x => x.ID == id) : new UserModel();

        public UserModel? Get(Expression<Func<UserModel, bool>> predicate) => Users.Any(predicate) ? Users.FirstOrDefault(predicate) : new();

        public ExceptionModel Update(UserModel item)
        {
            if (item is null)
                return ExceptionModel.OperationFailed;
            if (Get(item.ID) is null)
                return ExceptionModel.OperationFailed;
            Users.Update(item);
            SaveChanges();
            return ExceptionModel.Successfull;
        }

        string IUserRepository<UserModel>.HashPassword(string password) => HashPassword(password);

        ExceptionModel IRepository<UserModel>.Update(UserModel item)
        {
            if (item is null)
                return ExceptionModel.OperationFailed;
            if (Get(item.ID) is null)
                return ExceptionModel.OperationFailed;
            Users.Update(item);
            SaveChanges();
            return ExceptionModel.Successfull;
        }
    }
}
