using System.Text.Json;

namespace CabManagementSystem.Models
{
    /// <summary>
    /// model for functions handling in admin panel
    /// </summary>
    public class AdminHandlingModel
    {
        public int ID { get; set; }
        public Guid UserID { get; set; } = new();
        public SelectModeEnum SelectMode { get; set; } = SelectModeEnum.Default;

    }

    /// <summary>
    /// defines model of time in which was cab ordered
    /// </summary>
    public class OrderTimeModel
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public Guid UserID { get; set; } = new();
        public DateTime ArrivingTime { get; set; } = new DateTime();
        public DateTime CurrentTime { get; set; } = DateTime.Now;


        /// <summary>
        /// serialize data of order's time from json format
        /// </summary>
        /// <param name="data"></param>
        /// <param name="path"></param>
        /// <exception cref="Exception"></exception>
        public static void SerializeOrderTimeData(OrderTimeModel data, string path)
        {
            var listOrderTimes = JsonSerializer.Deserialize<RootObjectOrderTimeModel>(File.ReadAllText(path));
            if (listOrderTimes is null)
                throw new ArgumentNullException("variable listOrderTimes is null.");
            listOrderTimes.ListOrderTimes.Add(data);
            File.WriteAllText(path, JsonSerializer.Serialize(listOrderTimes)); // need to add whole instance of RootObjectOrdertimeModel rather than only ListOrderTimes
        }


        /// <summary>
        /// deserialize data of order's time from json format
        /// </summary>
        /// <param name="path"></param>
        /// <returns>list of model OrderTimeModel</returns>
        public static List<OrderTimeModel> DeserializeTaxiData(string path) => JsonSerializer.Deserialize<RootObjectOrderTimeModel>(File.ReadAllText(path)).ListOrderTimes;
    }

    /// <summary>
    /// defines object for serializing model OrderTimeModel
    /// </summary>
    public class RootObjectOrderTimeModel
    {
        public List<OrderTimeModel> ListOrderTimes { get; set; } = new();
    }

    /// <summary>
    /// defines the type of the select id input
    /// </summary>
    public enum SelectModeEnum
    {
        Default,
        BySelect,
        ByInput
    }
}
