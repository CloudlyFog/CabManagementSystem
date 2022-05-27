namespace CabManagementSystem.Models
{
    public class AdminHandlingModel
    {
        public int ID { get; set; }
        public Guid UserID { get; set; } = new();
        public SelectIDInput SelectID { get; set; } = SelectIDInput.Default;
    }

    /// <summary>
    /// defines the type of the select id input
    /// </summary>
    public enum SelectIDInput
    {
        Default,
        BySelect,
        ByInput
    }
}
