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
        public int ID { get; set; }
        public DateTime Time { get; set; } = DateTime.Now;
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
