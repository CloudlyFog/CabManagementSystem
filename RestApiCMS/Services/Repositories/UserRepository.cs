
using CabManagementSystem.AppContext;
using CabManagementSystem.Models;
using CabManagementSystem.Services.Interfaces;
using System.Linq.Expressions;
using BankAccountModel = BankSystem.Models.BankAccountModel;

namespace RestApiCMS.Services.Repositories
{
    public class UserRepository : ApplicationContext, IRepository<UserModel>
    {
        private readonly IBankAccountRepository<BankAccountModel> bankAccountRepository;
        public UserRepository()
        {
            bankAccountRepository = new CabManagementSystem.Services.Repositories.BankAccountRepository();
        }
        public ExceptionModel Create(UserModel item)
        {
            if (Get(item.ID) is null)//if user isn`t exist method will exit with exception OperationFailed
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
            return bankAccountRepository.Create(bankAccountModel);
        }

        public ExceptionModel Delete(UserModel item)
        {
            if (item is null)
                return ExceptionModel.OperationFailed;
            if (Get(item.ID) is null)
                return ExceptionModel.OperationFailed;
            Users.Remove(item);
            SaveChanges();
            var bankAccountModel = bankAccountRepository.Get(x => x.ID == item.ID);
            return bankAccountRepository.Delete(bankAccountModel);
        }

        public bool Exist(Guid id) => Users.Any(user => user.ID == id && user.Authenticated);

        public bool Exist(Expression<Func<UserModel, bool>> predicate) => Users.Any(predicate);

        public IEnumerable<UserModel> Get() => Users.Any() ? Users.AsEnumerable() : new List<UserModel>();

        public UserModel Get(Guid id) => Users.Any(x => x.ID == id) ? Users.First(x => x.ID == id) : new UserModel();

        public UserModel? Get(Expression<Func<UserModel, bool>> predicate) => Users.FirstOrDefault(predicate);

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
    }
}
