using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CabManagementSystem.Models
{
    public class OrderModel
    {
        [Key]
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
    }
}
