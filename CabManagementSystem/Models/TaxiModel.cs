using System.ComponentModel.DataAnnotations;

namespace CabManagementSystem.Models
{
    public class TaxiModel
    {
        [Key]
        public int Id { get; set; }
        public string TaxiNumber { get; set; } = string.Empty;
        public TaxiClass TaxiClass { get; set; }
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
