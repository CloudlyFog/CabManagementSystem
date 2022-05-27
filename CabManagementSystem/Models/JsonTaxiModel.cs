namespace CabManagementSystem.Models
{
    public class JsonTaxiModel : TaxiModel
    {
        public new List<TaxiModel> TaxiList { get; set; } = new();
    }
}
