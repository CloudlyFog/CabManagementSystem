using CabManagementSystem.Services.Repositories;

namespace CMSTest
{
    public class Tests
    {
        private readonly OrderRepository orderRepository = new();
        private readonly OrderRepository fsdfs = new();

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestFilter()
        {
            var s = orderRepository.Filter();
            Assert.Equals(orderRepository, fsdfs);
        }
    }
}