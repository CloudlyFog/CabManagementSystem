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
        public Guid BankID { get; set; } = new();
        public decimal BankAccountAmount { get; set; }

        [NotMapped]
        public ExceptionModel Exception { get; set; } = ExceptionModel.Successfull;

        [NotMapped]
        public OrderModel? Order { get; set; } = new();

        [NotMapped]
        public TaxiModel? Taxi { get; set; } = new();

        [NotMapped]
        public DriverModel? Driver { get; set; } = new();

        [NotMapped]
        public BankAccountModel? BankAccount { get; set; } = new();
    }

    /// <summary>
    /// defines model of possible exceptions of methods'es returns
    /// </summary>
    public enum ExceptionModel
    {
        VariableIsNull = 100,
        Successfull = 200,
        OperationRestricted = 300,
        OperationFailed = 400,
        OperationNotExist = 401
    }
}
