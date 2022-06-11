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
        public bool Authenticated { get; set; }
        public bool Access { get; set; }
        public bool HasOrder { get; set; }
        public Guid BankAccountID { get; set; } = Guid.NewGuid();
        public decimal BankAccountAmount { get; set; } = 0;

        [NotMapped]
        public BankModel? BankModel { get; set; } = new();

        [NotMapped]
        public OrderModel? Order { get; set; } = new();

        [NotMapped]
        public TaxiModel? Taxi { get; set; } = new();

        [NotMapped]
        public DriverModel? Driver { get; set; } = new();
    }
}
