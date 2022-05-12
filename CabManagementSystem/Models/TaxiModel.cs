using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CabManagementSystem.Models
{
    public class TaxiModel
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();
        public Guid DriverID { get; set; } = Guid.NewGuid();
        public string TaxiNumber { get; set; } = string.Empty;
        public TaxiClass TaxiClass { get; set; }

        [NotMapped]
        public BindTaxiDriver BindTaxiDriver { get; set; } = new();
    }

    /// <summary>
    /// defines taxi's class as example buisness, comfort, premium and etc
    /// </summary>
    public enum TaxiClass
    {
        Buisness = 1,
        Comfort,
        Premium,
        Economy
    }
}
