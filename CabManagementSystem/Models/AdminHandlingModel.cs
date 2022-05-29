using System.Text.Json;

namespace CabManagementSystem.Models
{
    public class AdminHandlingModel
    {
        public int ID { get; set; }
        public Guid UserID { get; set; } = new();
        public SelectModeEnum SelectMode { get; set; } = SelectModeEnum.Default;
        public DateTime Time { get; set; } = DateTime.Now;
        public List<OrderTimeModel> TimeList { get; set; } = new();

    }

    /// <summary>
    /// defines model of time in which was cab ordered
    /// </summary>
    public class OrderTimeModel
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public DateTime Time { get; set; } = DateTime.Now;



        public static void SerializeOrderTimeData(OrderTimeModel data, string path)
        {
            var listOrderTimes = JsonSerializer.Deserialize<RootObjectOrdertimeModel>(File.ReadAllText(path)).ListOrderTimes;
            if (listOrderTimes is null)
                throw new Exception("variable listOrderTimes is null.");
            listOrderTimes.Add(data);
            File.Delete(path);
            File.WriteAllText(path, JsonSerializer.Serialize(listOrderTimes)); // need to add whole instance of RootObjectOrdertimeModel rather than only ListOrderTimes
        }


        /// <summary>
        /// deserialize data of order's time from json format
        /// </summary>
        /// <param name="path"></param>
        /// <returns>list of model OrderTimeModel</returns>
        public static List<OrderTimeModel> DeserializeTaxiData(string path) => JsonSerializer.Deserialize<RootObjectOrdertimeModel>(File.ReadAllText(path)).ListOrderTimes;
    }

    public class RootObjectOrdertimeModel : OrderTimeModel
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
