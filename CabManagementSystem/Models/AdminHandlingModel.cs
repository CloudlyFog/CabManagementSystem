using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        [NotMapped, JsonIgnore]
        public List<OrderTimeModel> ListOrderTimes { get; set; } = new();

        public static void SerializeOrderTimeData(OrderTimeModel data, string path)
        {
            var jsonDes = JsonSerializer.Deserialize<OrderTimeModel>(File.ReadAllText(path));
            if (jsonDes is null)
                throw new Exception("variable jsonDes is null.");
            jsonDes.ListOrderTimes.Add(data);
            //File.Delete(path);
            File.WriteAllText(path, JsonSerializer.Serialize(jsonDes.ListOrderTimes));
        }


        /// <summary>
        /// deserialize data of order's time from json format
        /// </summary>
        /// <param name="path"></param>
        /// <returns>list of model OrderTimeModel</returns>
        public static List<OrderTimeModel> DeserializeTaxiData(string path) => JsonSerializer.Deserialize<OrderTimeModel>(File.ReadAllText(path)).ListOrderTimes;
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
