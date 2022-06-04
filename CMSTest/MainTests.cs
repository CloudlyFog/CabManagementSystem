using CabManagementSystem.AppContext;
using CabManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CMSTest
{
    public class Tests
    {
        private const string PathSerializationJsonAjax = "D:/CabManagementSystem/CabManagementSystem/Data/Json/taxi.json";
        private const string pathOrderTime = "D:/CabManagementSystem/CabManagementSystem/Data/Json/time.json";
        private readonly ApplicationContext applicationContext = new(new DbContextOptions<ApplicationContext>());
        [SetUp]
        public void Setup()
        {
        }
        [Test]
        public void UpdateData()
        {
            var actual = new OrderTimeModel()
            {
                ID = new("c62d43f7-ee3e-4f34-b337-7100970fa87b"),
                UserID = new("A08AB3E5-E3EC-47CD-84EF-C0EB75045A70"),
                ArrivingTime = DateTime.Parse("2022-06-04T10:05:45.5600099+03:00")
            };
            var updated = new OrderTimeModel()
            {
                ID = new("c62d43f7-ee3e-4f34-b337-7100970fa87b"),
                UserID = new("A08AB3E5-E3EC-47CD-84EF-C0EB75045A70"),
                ArrivingTime = DateTime.Parse("2022-06-04T10:07:45.5600099+03:00"),
                CurrentTime = DateTime.Parse("2022-06-04T10:05:45.5600099+03:00")
            };
            OrderTimeModel.UpdateSerializeData(updated, pathOrderTime, actual.ID);
            Assert.Multiple(() =>
            {
                Assert.That(actual.ID, Is.EqualTo(updated.ID));
                Assert.That(actual.ArrivingTime, Is.EqualTo(updated.ArrivingTime));
                Assert.That(actual.UserID, Is.EqualTo(updated.UserID));
            });
        }

        [Test]
        public void SerializeOrderTime()
        {
            var expected = new OrderTimeModel()
            {
                ID = new("c62d43f7-ee3e-4f34-b337-7100970fa87b"),
                UserID = new("A08AB3E5-E3EC-47CD-84EF-C0EB75045A70"),
                ArrivingTime = DateTime.Parse("2022-06-04T10:05:45.5600099+03:00")
            };
            OrderTimeModel.SerializeOrderTimeData(expected, pathOrderTime);
            var actual = OrderTimeModel.DeserializeTaxiData(pathOrderTime)[0];
            Assert.Multiple(() =>
                    {
                        Assert.That(actual.ID, Is.EqualTo(expected.ID));
                        Assert.That(actual.ArrivingTime, Is.EqualTo(expected.ArrivingTime));
                        Assert.That(actual.UserID, Is.EqualTo(expected.UserID));
                    });
        }

        [Test]
        public void Serialization()
        {
            var expected = new TaxiModel()
            {
                ID = new("f6c6ed2d-86b4-41b4-af23-4f1f0915b665"),
                DriverID = new("bd1e063f-b343-4387-812c-e203bfaa1f65"),
                TaxiNumber = "�332��46",
                TaxiClass = TaxiClass.Comfort
            };

            // Serialization data
            applicationContext.SerializeData(expected, pathOrderTime);

            var list = applicationContext.DeserializeTaxiData(pathOrderTime);
            var actual = list[0];

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
                TaxiNumber = "�032��36",
                TaxiClass = TaxiClass.Comfort
            };
            var list = applicationContext.DeserializeTaxiData(PathSerializationJsonAjax);
            var actual = list[0];
            Assert.AreEqual(expected.ID, actual.ID);
            Assert.AreEqual(expected.DriverID, actual.DriverID);
            Assert.AreEqual(expected.TaxiNumber, actual.TaxiNumber);
            Assert.AreEqual(expected.TaxiClass, actual.TaxiClass);
        }
    }
}