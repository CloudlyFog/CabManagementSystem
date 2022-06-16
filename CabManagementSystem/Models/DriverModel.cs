using System.ComponentModel.DataAnnotations;

namespace CabManagementSystem.Models
{
    public class DriverModel
    {
        [Key]
        public Guid DriverID { get; set; } = Guid.NewGuid();
        public Guid TaxiID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool Busy { get; set; }
        public TaxiPrice TaxiPrice { get; set; }
    }
}
