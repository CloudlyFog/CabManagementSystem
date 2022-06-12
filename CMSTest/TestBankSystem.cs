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
            var expected = new UserModel()
            {
                ID = id,
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
            var expected = new UserModel()
            {
                ID = id,
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
            var operation = bankContext.Operations.FirstOrDefault(x => x.ID == new Guid("AE734776-9CB6-464E-9ADF-638A04DB8E0F"));
            bankContext.CreateOperation(operation, OperationKind.Accrual);
        }
    }
}
