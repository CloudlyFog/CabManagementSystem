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
        public void UpdateCurrentTime()
        {
            var actual = new OrderTimeModel()
            {
                ID = new("c62d43f7-ee3e-4f34-b337-7100970fa87b"),
                UserID = new("A08AB3E5-E3EC-47CD-84EF-C0EB75045A70"),
                ArrivingTime = DateTime.Parse("2022-06-04T10:05:45.5600099+03:00"),
                CurrentTime = DateTime.Parse("2022-06-04T10:00:45.5600099+03:00")
            };
            var updated = new OrderTimeModel()
            {
                ID = new("c62d43f7-ee3e-4f34-b337-7100970fa87b"),
                UserID = new("A08AB3E5-E3EC-47CD-84EF-C0EB75045A70"),
                ArrivingTime = DateTime.Parse("2022-06-04T10:05:45.5600099+03:00"),
                CurrentTime = DateTime.Parse("2022-06-04T10:00:45.5600099+03:00")
            };
            OrderTimeModel.UpdateTime(pathOrderTime, actual.ID, updated.CurrentTime, updated.ArrivingTime = default);
            Assert.That(actual.CurrentTime, Is.EqualTo(updated.CurrentTime));
        }

        [Test]
        public void UpdateArrivingTime()
        {
            var actual = new OrderTimeModel()
            {
                ID = new("c62d43f7-ee3e-4f34-b337-7100970fa87b"),
                UserID = new("A08AB3E5-E3EC-47CD-84EF-C0EB75045A70"),
                ArrivingTime = DateTime.Parse("2022-06-04T10:07:45.5600099+03:00"),
                CurrentTime = DateTime.Parse("2022-06-04T10:00:45.5600099+03:00")
            };
            var updated = new OrderTimeModel()
            {
                ID = new("c62d43f7-ee3e-4f34-b337-7100970fa87b"),
                UserID = new("A08AB3E5-E3EC-47CD-84EF-C0EB75045A70"),
                ArrivingTime = DateTime.Parse("2022-06-04T10:07:45.5600099+03:00"),
                CurrentTime = DateTime.Parse("2022-06-04T10:00:45.5600099+03:00")
            };
            OrderTimeModel.UpdateTime(pathOrderTime, actual.ID, updated.CurrentTime = default, updated.ArrivingTime);
            Assert.That(actual.ArrivingTime, Is.EqualTo(updated.ArrivingTime));
        }

        [Test]
        public void UpdateOrderTime()
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
            OrderTimeModel.UpdateOrderTimeJson(updated, pathOrderTime, actual.ID);
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
            var actual = OrderTimeModel.DeserializeTimeModelJson(pathOrderTime)[0];
            Assert.Multiple(() =>
                    {
                        Assert.That(actual.ID, Is.EqualTo(expected.ID));
                        Assert.That(actual.ArrivingTime, Is.EqualTo(expected.ArrivingTime));
                        Assert.That(actual.UserID, Is.EqualTo(expected.UserID));
                    });
        }
    }
}