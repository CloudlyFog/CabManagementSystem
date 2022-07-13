using CabManagementSystem.AppContext;

namespace CMSTest
{
    public class Tests
    {
        private const string PathSerializationJsonAjax = "D:/CabManagementSystem/CabManagementSystem/Data/Json/taxi.json";
        private const string pathOrderTime = "D:/CabManagementSystem/CabManagementSystem/Data/Json/time.json";
        private readonly ApplicationContext applicationContext = new();
        [SetUp]
        public void Setup()
        {
        }

    }
}