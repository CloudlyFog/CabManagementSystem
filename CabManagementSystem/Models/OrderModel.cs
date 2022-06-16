using System.ComponentModel.DataAnnotations;

namespace CabManagementSystem.Models
{
    public class OrderModel
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();
        public Guid UserID { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string DriverName { get; set; } = string.Empty;
        public TaxiPrice Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime OrderTime { get; set; } = DateTime.Now;

    }
}
