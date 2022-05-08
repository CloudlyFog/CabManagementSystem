using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CabManagementSystem.Models
{
    public class OrderModel
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();
        public Guid UserID { get; set; } = new();
        public string PhoneNumber { get; set; } = string.Empty;
        public string DriverName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
