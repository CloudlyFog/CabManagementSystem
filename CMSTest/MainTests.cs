using CabManagementSystem.AppContext;
using CabManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CMSTest
{
    public class Tests
    {
        private const string PathSerializationJsonAjax = "D:/CabManagementSystem/CabManagementSystem/Data/Json/taxi.json";
        private readonly ApplicationContext applicationContext = new(new DbContextOptions<ApplicationContext>());
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Serialization()
        {
            var expected = new TaxiModel()
            {
                ID = new("f6c6ed2d-86b4-41b4-af23-4f1f0915b665"),
                DriverID = new("bd1e063f-b343-4387-812c-e203bfaa1f65"),
                TaxiNumber = "À332ÊÐ46",
                TaxiClass = TaxiClass.Comfort
            };

            // Serialization data
            applicationContext.SerializeData(expected, PathSerializationJsonAjax);

            var list = applicationContext.DeserializeData(PathSerializationJsonAjax);
            var actual = list[2];

            // tests
            Assert.AreEqual(expected.ID, actual.ID);
            Assert.AreEqual(expected.DriverID, actual.DriverID);
            Assert.AreEqual(expected.TaxiNumber, actual.TaxiNumber);
            Assert.AreEqual(expected.TaxiClass, actual.TaxiClass);
        }

        [Test]
        public void Deserialization()
        {
            var expected = new TaxiModel()
            {
                ID = new("f6c6ed2d-86b4-41b4-af23-4f1f0915b665"),
                DriverID = new("bd1e063f-b343-4387-812c-e203bfaa1f65"),
                TaxiNumber = "À032ÊÐ36",
                TaxiClass = TaxiClass.Comfort
            };
            var list = applicationContext.DeserializeData(PathSerializationJsonAjax);
            var actual = list[0];
            Assert.AreEqual(expected.ID, actual.ID);
            Assert.AreEqual(expected.DriverID, actual.DriverID);
            Assert.AreEqual(expected.TaxiNumber, actual.TaxiNumber);
            Assert.AreEqual(expected.TaxiClass, actual.TaxiClass);
        }
    }
}