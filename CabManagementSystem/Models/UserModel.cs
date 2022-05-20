using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CabManagementSystem.Models
{
    public class UserModel
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool Authenticated { get; set; } = false;
        public bool Access { get; set; } = false;


        [NotMapped]
        public OrderModel? Order { get; set; } = new();

        [NotMapped]
        public TaxiModel? Taxi { get; set; } = new();

        [NotMapped]
        public DriverModel? Driver { get; set; } = new();

    }
}
