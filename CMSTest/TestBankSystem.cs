using CabManagementSystem.AppContext;
using CabManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CMSTest
{
    internal class TestBankSystem
    {
        private readonly BankAccountContext bankAccountContext = new(new DbContextOptions<BankAccountContext>());
        private readonly BankContext bankContext = new(new DbContextOptions<BankContext>());
        private readonly Guid id = new("A08AB3E5-E3EC-47CD-84EF-C0EB75045A70");

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Accraul()
        {
            var expected = new BankAccountModel()
            {
                UserBankAccountID = id,
                BankAccountAmount = 200
            };

            bankAccountContext.Accrual(expected, 100);

            var actual = new UserModel()
            {
                ID = id,
                BankAccountAmount = bankAccountContext.Users.FirstOrDefault(x => x.ID == id).BankAccountAmount
            };

            Assert.That(actual.BankAccountAmount, Is.EqualTo(expected.BankAccountAmount));
        }

        [Test]
        public void Withdraw()
        {
            var expected = new BankAccountModel()
            {
                UserBankAccountID = id,
                BankAccountAmount = 0
            };

            bankAccountContext.Withdraw(expected, 100);

            var actual = new UserModel()
            {
                ID = id,
                BankAccountAmount = bankAccountContext.Users.FirstOrDefault(x => x.ID == id).BankAccountAmount
            };

            Assert.That(actual.BankAccountAmount, Is.EqualTo(expected.BankAccountAmount));
        }

        [Test]
        public void Operations()
        {
            var bankAccount = bankContext.BankAccounts.First(x => x.BankID == new Guid("bed62930-9356-477a-bed5-b84d59336122"));
            var user = bankContext.Users.First(x => x.ID == id);
            var bank = bankContext.Banks.First(x => x.BankID == new Guid("bed62930-9356-477a-bed5-b84d59336122"));
            var operation = bankContext.Operations.First(x => x.ID == new Guid("AE734776-9CB6-464E-9ADF-638A04DB8E0F"));
            bankContext.BankAccrual(bankAccount, bank, operation);
            Assert.That(bankContext.Banks.First(x => x.BankID == new Guid("bed62930-9356-477a-bed5-b84d59336122")).AccountAmount - 120, Is.EqualTo(bank.AccountAmount));
        }
    }
}
