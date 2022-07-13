using BankSystem.Models;

namespace RestApiCMS.Services.Interfaces
{
    public interface IBankAccountRepository<T> : IRepository<T> where T : class
    {
        ExceptionModel Accrual(BankAccountModel item, decimal amountAccrual);
        ExceptionModel Withdraw(BankAccountModel item, decimal amountAccrual);
        ExceptionModel Update(BankAccountModel item, UserModel user);
    }
    public interface IBankRepository<T> : IBankAccountRepository<T> where T : class
    {
        ExceptionModel BankAccountAccrual(BankAccountModel bankAccount, BankModel bank, OperationModel operation);
        ExceptionModel BankAccountWithdraw(BankAccountModel bankAccount, BankModel bank, OperationModel operation);
    }
}
