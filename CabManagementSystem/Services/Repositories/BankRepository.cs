using BankSystem.AppContext;
using BankSystem.Models;
using CabManagementSystem.Services.Interfaces;
using System.Linq.Expressions;

namespace CabManagementSystem.Services.Repositories
{
    public class BankRepository : BankContext, IBankRepository<BankModel>
    {
        public ExceptionModel Accrual(BankAccountModel item, decimal amountAccrual)
        {
            throw new NotImplementedException();
        }

        public new ExceptionModel BankAccountAccrual(BankAccountModel bankAccount, BankModel bank, OperationModel operation)
        {
            throw new NotImplementedException();
        }

        public new ExceptionModel BankAccountWithdraw(BankAccountModel bankAccount, BankModel bank, OperationModel operation)
        {
            throw new NotImplementedException();
        }

        public Models.ExceptionModel Create(BankModel item)
        {
            throw new NotImplementedException();
        }

        public Models.ExceptionModel Delete(BankModel item)
        {
            throw new NotImplementedException();
        }

        public bool Exist(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool Exist(Expression<Func<BankModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BankModel> Get() => Banks;

        public BankModel Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public BankModel Get(Expression<Func<BankModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public ExceptionModel Update(BankAccountModel item, UserModel user)
        {
            throw new NotImplementedException();
        }

        public Models.ExceptionModel Update(BankModel item)
        {
            throw new NotImplementedException();
        }

        public ExceptionModel Withdraw(BankAccountModel item, decimal amountAccrual)
        {
            throw new NotImplementedException();
        }
    }
}
