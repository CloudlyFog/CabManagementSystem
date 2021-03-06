using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CabManagementSystem.Models
{
    public class TaxiModel
    {
        [Key]
        public Guid ID { get; set; } = new();
        public Guid DriverID { get; set; } = Guid.NewGuid();
        public string TaxiNumber { get; set; } = string.Empty;
        public TaxiClass TaxiClass { get; set; }
        public bool Busy { get; set; }

        [NotMapped]
        public List<TaxiModel> TaxiList { get; set; } = new();

        [NotMapped]
        public BindTaxiDriver BindTaxiDriver { get; set; } = new();
    }

    /// <summary>
    /// defines taxi's class as example buisness, comfort, premium and etc
    /// </summary>
    public enum TaxiClass
    {
        Buisness = 1,
        Premium,
        Comfort,
        Economy
    }

    /// <summary>
    /// defines taxi's price for class of taxi
    /// </summary>
    public enum TaxiPrice
    {
        Buisness = 260,
        Premium = 235,
        Comfort = 180,
        Economy = 150
    }
}
