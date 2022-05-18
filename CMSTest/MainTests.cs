using CabManagementSystem.AppContext;
using CabManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CMSTest
{
    public class Tests
    {
        private const string PathSerializeJsonAjax = "D:/CabManagementSystem/CabManagementSystem/Data/Json/taxi.json";
        private readonly ApplicationContext applicationContext = new(new DbContextOptions<ApplicationContext>());
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Serialization()
        {
            var taxi = new TaxiModel()
            {

            };
            Assert.True(applicationContext.SerializeData(PathSerializeJsonAjax));
        }

        [Test]
        public void Deserialization()
        {

            Assert.True(applicationContext.SerializeData(PathSerializeJsonAjax));
        }
    }
}