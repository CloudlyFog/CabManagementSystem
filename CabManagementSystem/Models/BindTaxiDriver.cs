namespace CabManagementSystem.Models
{
    public class BindTaxiDriver
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public Guid TaxiID { get; set; } = Guid.NewGuid();
        public Guid DriverID { get; set; } = Guid.NewGuid();
    }
}
