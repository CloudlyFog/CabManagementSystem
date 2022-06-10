namespace CabManagementSystem.Models
{
    public class BankModel
    {
        public Guid ID { get; set; } = Guid.NewGuid(); // id for identification in the database
        public Guid BankID { get; set; }
        public string BankName { get; set; } = string.Empty;
    }
}
