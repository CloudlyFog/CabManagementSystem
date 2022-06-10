using System.ComponentModel.DataAnnotations;

namespace CabManagementSystem.Models
{
    public class DriverModel
    {
        [Key]
        public Guid DriverID { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool Busy { get; set; }
    }
}
