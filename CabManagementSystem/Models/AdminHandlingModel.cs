namespace CabManagementSystem.Models
{
    public class AdminHandlingModel
    {
        public int ID { get; set; }
        public Guid UserID { get; set; } = new();
        public SelectModeEnum SelectMode { get; set; } = SelectModeEnum.Default;
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
